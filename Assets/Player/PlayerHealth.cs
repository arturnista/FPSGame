using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	public static PlayerHealth Instance;

	public float health {
		get {
			return m_CurrentHealth;
		}
	}
	public bool godMode;

	private PlayerMovement m_Movement;

	[SerializeField]
	private float m_MaxHealth;
	private float m_CurrentHealth;

	private HUDController m_HUDController;

	void Awake () {
		Instance = this;
		m_CurrentHealth = m_MaxHealth;
		m_HUDController = GameObject.FindObjectOfType<HUDController>();
		m_Movement = GetComponent<PlayerMovement>();
	}
	
	void Update () {
		
	}

	public void GiveHealth(float amount) {
		m_CurrentHealth += amount;
		if(m_CurrentHealth > m_MaxHealth) m_CurrentHealth = m_MaxHealth;
	}

	public void TakeDamage(float damage) {
		TakeDamage(damage, Vector3.zero);
	}

	public void TakeDamage(float damage, Vector3 pushVelocity) {
		if(!godMode) m_CurrentHealth -= damage;
		Debug.Log("DAMAGE_TAKEN|"+damage);

		if(pushVelocity != Vector3.zero) m_Movement.AddVelocity(pushVelocity);
		
		if(m_CurrentHealth <= 0f) {
			m_HUDController.Die();
		} else {
			m_HUDController.TakeDamage();
		}
	}
}
