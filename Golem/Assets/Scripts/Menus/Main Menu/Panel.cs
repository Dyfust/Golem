using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Panel : MonoBehaviour
{
	[SerializeField] private GameObject _defaultSelectedObject; 

	public void OnOpen()
	{
		EventSystem.current.SetSelectedGameObject(_defaultSelectedObject); 
	}

	public GameObject GetDefaultSelectedObject()
	{
		return _defaultSelectedObject; 
	}
}
