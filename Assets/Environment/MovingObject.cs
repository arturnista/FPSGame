using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour {

	public Vector3 finalPosition;
	public float duration;

	private Vector3 m_InitialPosition;
	private float m_InitialTime;

	private bool m_GoingToFinal;

	void Awake() {
		m_InitialPosition = transform.position;
		m_InitialTime = Time.time;

		m_GoingToFinal = true;
	}

	void Update() {
		float t = (Time.time - m_InitialTime) / duration;
		if(m_GoingToFinal) transform.position = Vector3.Lerp(m_InitialPosition, finalPosition, t);
		else transform.position = Vector3.Lerp(finalPosition, m_InitialPosition, t);			

		if(t >= 1f) {
			m_GoingToFinal = !m_GoingToFinal;
			m_InitialTime = Time.time;
		}
	}
	
}
