using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour {

	public Vector3 finalPosition;
	public float duration;
	public bool startActive = true;

	private Vector3 m_InitialPosition;
	private float m_InitialTime;

	private bool m_GoingToFinal;
	private bool m_Active;

	private Light m_Light;

	void Awake() {
		m_Light = GetComponentInChildren<Light>();

		m_InitialPosition = transform.position;
		m_InitialTime = Time.time;

		m_GoingToFinal = true;
		if(startActive) On();
		else Off();
	}

	void Update() {
		if(!m_Active) return;

		float t = (Time.time - m_InitialTime) / duration;
		if(m_GoingToFinal) transform.position = Vector3.Lerp(m_InitialPosition, finalPosition, t);
		else transform.position = Vector3.Lerp(finalPosition, m_InitialPosition, t);			

		if(t >= 1f) {
			m_GoingToFinal = !m_GoingToFinal;
			m_InitialTime = Time.time;
		}
	}

	public void On() {
		m_Active = true;
		m_Light.enabled = true;
	}

	public void Off() {
		m_Active = false;
		m_Light.enabled = false;
	}
	
}
