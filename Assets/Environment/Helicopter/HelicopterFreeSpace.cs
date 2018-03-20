using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterFreeSpace : MonoBehaviour {

	private int m_EnemiesInSpace;
	private bool m_IsWaiting;

	private float m_TimeToCheck;
	private BoxCollider m_BoxCollider;
	private HelicopterBehaviour m_Helicopter;

	void Start () {
		m_IsWaiting = false;
		m_BoxCollider = GetComponent<BoxCollider>();
		m_Helicopter = GameObject.FindObjectOfType<HelicopterBehaviour>();
	}
	
	void Update () {

		if(m_IsWaiting) {
			if(m_TimeToCheck < Time.time) {
				m_TimeToCheck = Time.time + 2f;

				m_EnemiesInSpace = 0;
				Collider[] colliders = Physics.OverlapBox(transform.position, m_BoxCollider.size / 2f, Quaternion.identity);
				Debug.Log(colliders.Length);
				foreach(Collider c in colliders) {
					EnemyMovement en = c.GetComponent<EnemyMovement>();
					if(en) m_EnemiesInSpace++;
				}
			
				if(m_EnemiesInSpace == 0) {
					m_Helicopter.Land();
					Destroy(gameObject);
				}
			}
		}	
	}

	public void StartWaiting() {
		m_IsWaiting = true;
		m_TimeToCheck = Time.time + 2f;
	}

}
