using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
	
	public GameObject impactEffectPrefab;

	private Transform m_Head;

	void Awake () {
		m_Head = transform.Find("Head");
		
	}
	
	void Update () {
		if(Input.GetMouseButtonDown(0)) {
			Shoot();
		}
	}

	void Shoot() {
		RaycastHit hit;

		Debug.DrawRay(m_Head.transform.position, m_Head.transform.forward * 10f, Color.red, 10f);
		if(Physics.Raycast(m_Head.transform.position, m_Head.transform.forward, out hit)) {
			Instantiate(impactEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
		}
	}
}
