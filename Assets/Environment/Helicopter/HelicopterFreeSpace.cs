using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterFreeSpace : MonoBehaviour {

	public GameObject explosionPrefab;
	public GameObject boss;

	private int m_EnemiesInSpace;
	private bool m_IsWaiting;

	private float m_TimeToCheck;
	private BoxCollider m_BoxCollider;
	private HelicopterBehaviour m_Helicopter;
	private HUDController m_HUDController;

	void Start () {
		m_IsWaiting = false;
		m_BoxCollider = GetComponent<BoxCollider>();
		m_Helicopter = GameObject.FindObjectOfType<HelicopterBehaviour>();
		m_HUDController = GameObject.FindObjectOfType<HUDController>();
	}
	
	void Update () {

		if(m_IsWaiting) {
			if(m_TimeToCheck < Time.time) {
				m_TimeToCheck = Time.time + 2f;

				bool isPayload = false;
				m_EnemiesInSpace = 0;
				Collider[] colliders = Physics.OverlapBox(transform.position, m_BoxCollider.size / 2f, Quaternion.identity);
				foreach(Collider c in colliders) {
					EnemyMovement en = c.GetComponent<EnemyMovement>();
					if(en) m_EnemiesInSpace++;
					else {
						PayloadBehaviour pl = c.GetComponent<PayloadBehaviour>();
						if(pl) isPayload = true;
					}
				}
			
				if(isPayload && m_EnemiesInSpace == 0) {
					m_Helicopter.Land();
					m_IsWaiting = false;
				}
			}
		}	
	}

	public void Explode() {
		Instantiate(explosionPrefab, m_Helicopter.landPosition, Quaternion.identity);
		Destroy(m_Helicopter.gameObject);
		boss.SetActive(true);
	}

	public void StartWaiting() {
		m_IsWaiting = true;
		m_TimeToCheck = Time.time + 2f;

		m_HUDController.ShowText("Clean the field for the helicopter land!");
	}

}
