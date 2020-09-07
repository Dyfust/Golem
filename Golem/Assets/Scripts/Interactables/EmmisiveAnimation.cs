using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmmisiveAnimation : MonoBehaviour
{
	[SerializeField] private Texture[] _emissive;
	[SerializeField] private MeshRenderer _presurePlateRen;

	private Material _pressurePlateMat;
	// Start is called before the first frame update
	void Start()
	{
		_pressurePlateMat = _presurePlateRen.material;


	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			StartCoroutine("AnimateEmissiveOut"); 
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			StartCoroutine("AnimateEmissiveIn"); 
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag.Equals("Orb"))
		{
			StartCoroutine("AnimateEmissiveOut"); 
		}
	}

	IEnumerator AnimateEmissiveOut()
	{
		for (int i = 0; i < _emissive.Length; i++)
		{
			_pressurePlateMat.SetTexture("_EmissionMap", _emissive[i]);
			yield return new WaitForSeconds(0.1f); 
		}
	}

	IEnumerator AnimateEmissiveIn()
	{
		for (int i = _emissive.Length - 1; i > -1; i--)
		{
			_pressurePlateMat.SetTexture("_EmissionMap", _emissive[i]);
			yield return new WaitForSeconds(0.1f);
		}
	}

}
