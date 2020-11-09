using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PanelManager : MonoBehaviour
{
	[SerializeField] private Panel[] _panels;

	[Tooltip("The panel which will be first viewed on start up")]
	[SerializeField] private Panel _defaultPanel;


	private void Start()
	{
		if (_defaultPanel != null)
			ActivatePanel(_defaultPanel);
	}

	private void Update()
	{
		if (EventSystem.current.currentSelectedGameObject == null)
		{
			foreach (Panel p in _panels)
			{
				if (p.gameObject.activeInHierarchy == true)
				{
					EventSystem.current.SetSelectedGameObject(p.GetDefaultSelectedObject());
				}
			}
		}
	}

	public void CloseAllPanels()
	{
		foreach (Panel p in _panels)
		{
			p.gameObject.SetActive(false); 
		}
	}

	public void ActivatePanel(Panel targetPanel)
	{
		foreach (Panel p in _panels)
		{
			if (p != targetPanel)
				p.gameObject.SetActive(false);
		}

		targetPanel.gameObject.SetActive(true);
		targetPanel.OnOpen();
	}

	public void Quit()
	{
		Application.Quit(); 
	}
}
