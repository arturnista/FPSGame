using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {
	
	[SerializeField]
	private float m_Duration;
	[SerializeField]	
	private Vector3 m_Translate;
	private Vector3 m_InitialPosition;
	private Vector3 m_FinalPosition;
	[SerializeField]	
	private Door m_InitialDoor;
	[SerializeField]	
	private Door m_FinalDoor;

	private bool m_IsInitialPosition;
	public bool isInitialPos {
		get {
			return m_IsInitialPosition;
		}
	}

	private bool m_IsActive;
	private float m_ActivateTime;

	private Vector3 m_Velocity;

	private PlayerMovement m_Player;

	void Awake () {
		m_IsInitialPosition = true;
		m_IsActive = false;
		m_InitialPosition = transform.position;
		m_FinalPosition = m_InitialPosition + m_Translate;

		m_Velocity = m_Translate / m_Duration;

		m_Player = GameObject.FindObjectOfType<PlayerMovement>();
	}
	
	void Update () {
		if(m_IsActive) {
			float time = ( Time.time - m_ActivateTime ) / m_Duration;
			if(m_IsInitialPosition) transform.position = Vector3.Lerp(m_InitialPosition, m_FinalPosition, time);
			else transform.position = Vector3.Lerp(m_FinalPosition, m_InitialPosition, time);

			if(time >= 1) {
				m_IsActive = false;
				m_IsInitialPosition = !m_IsInitialPosition;

				Deactivate();
			}
		}
	}

	public void Deactivate() {
		m_IsActive = false;

		if(m_IsInitialPosition) { if(m_InitialDoor) m_InitialDoor.Open(); }
		else { if(m_FinalDoor) m_FinalDoor.Open(); }

		m_Player.RemoveExtraVelocity(m_Velocity);
	}

	public void Activate() {
		if(m_IsActive) return;

		m_IsActive = true;
		m_ActivateTime = Time.time;

		m_Player.AddExtraVelocity(m_Velocity);

		m_InitialDoor.Close();
		m_FinalDoor.Close(); 
	}
}
