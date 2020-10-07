﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using Cinemachine;

public class CameraInputController : MonoBehaviour, AxisState.IInputAxisProvider, IPauseableObject
{
	private enum STATE
	{
		ACTIVE,
		INACTIVE
	}

	private STATE _currentState = STATE.ACTIVE;

	/// <summary>
	/// Leave this at -1 for single-player games.
	/// For multi-player games, set this to be the player index, and the actions will
	/// be read from that player's controls
	/// </summary>
	[Tooltip("Leave this at -1 for single-player games.  "
		+ "For multi-player games, set this to be the player index, and the actions will "
		+ "be read from that player's controls")]
	public int PlayerIndex = -1;

	/// <summary>Vector2 action for XY movement</summary>
	[Tooltip("Vector2 action for XY movement")]
	public InputActionReference XYAxis;

	/// <summary>Float action for Z movement</summary>
	[Tooltip("Float action for Z movement")]
	public InputActionReference ZAxis;

	float AxisState.IInputAxisProvider.GetAxisValue(int axis)
	{
		var action = ResolveForPlayer(axis, axis == 2 ? ZAxis : XYAxis);
		if (_currentState == STATE.INACTIVE)
		{
			switch (axis)
			{
				case 0: return 0; 
				case 1: return 0; 
				case 2: return 0; 
			}
		}
		else
		{
			if (action != null)
			{
				switch (axis)
				{
					case 0: return action.ReadValue<Vector2>().x;
					case 1: return action.ReadValue<Vector2>().y;
					case 2: return action.ReadValue<float>();
				}
			}
		}
		return 0;
	}

	const int NUM_AXES = 3;
	InputAction[] m_cachedActions;

	/// <summary>
	/// In a multi-player context, actions are associated with specific players
	/// This resolves the appropriate action reference for the specified player.
	/// 
	/// Because the resolution involves a search, we also cache the returned 
	/// action to make future resolutions faster.
	/// </summary>
	/// <param name="axis">Which input axis (0, 1, or 2)</param>
	/// <param name="actionRef">Which action reference to resolve</param>
	/// <returns>The cached action for the player specified in PlayerIndex</returns>
	protected InputAction ResolveForPlayer(int axis, InputActionReference actionRef)
	{
		if (axis < 0 || axis >= NUM_AXES)
			return null;
		if (actionRef == null || actionRef.action == null)
			return null;
		if (m_cachedActions == null || m_cachedActions.Length != NUM_AXES)
			m_cachedActions = new InputAction[NUM_AXES];
		if (m_cachedActions[axis] != null && actionRef.action.id != m_cachedActions[axis].id)
			m_cachedActions[axis] = null;
		if (m_cachedActions[axis] == null)
		{
			m_cachedActions[axis] = actionRef.action;
			if (PlayerIndex != -1)
			{
				var user = InputUser.all[PlayerIndex];
				m_cachedActions[axis] = user.actions.First(x => x.id == actionRef.action.id);
			}
		}
		// Auto-enable it if disabled
		if (m_cachedActions[axis] != null && !m_cachedActions[axis].enabled)
			m_cachedActions[axis].Enable();

		return m_cachedActions[axis];
	}

	// Clean up
	protected virtual void OnDisable()
	{
		m_cachedActions = null;
	}


	void IPauseableObject.Pause()
	{
		_currentState = STATE.INACTIVE;
	}

	void IPauseableObject.Resume()
	{
		_currentState = STATE.ACTIVE;
	}
}
