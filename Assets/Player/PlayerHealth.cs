using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	public float health {
		get {
			return m_CurrentHealth;
		}
	}

	[SerializeField]
	private float m_MaxHealth;
	private float m_CurrentHealth;

	private HUDController m_HUDController;

	void Awake () {
		m_CurrentHealth = m_MaxHealth;
		m_HUDController = GameObject.FindObjectOfType<HUDController>();
	}
	
	void Update () {
		
	}

	public void TakeDamage(float damage, EnemyMovement enemy) {
		m_CurrentHealth -= damage;
		m_HUDController.TakeDamage();
		
		if(m_CurrentHealth <= 0f) {
			Debug.Log("DED");
		}
	}
}
