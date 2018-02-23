using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
	
	public GameObject impactEffectPrefab;

	private Transform m_Head;
	private PlayerGun m_Gun;

	void Awake () {
		m_Head = transform.Find("Head");
		m_Gun = transform.GetComponentInChildren<PlayerGun>();
		
	}
	
	void Update () {
		if(Input.GetMouseButtonDown(0)) {
			m_Gun.StartShooting(m_Head);
		} else if(Input.GetMouseButtonUp(0)) {
			m_Gun.StopShooting();
		}

		if(Input.GetKeyDown(KeyCode.R)) {
			m_Gun.Reload();
		}
	}
}
