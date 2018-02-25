using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour {

	[SerializeField]
	private float m_FlickeringRange = .3f;
	[SerializeField]
	private float m_FlickeringRate = 2f;

	private Light m_Light;

	private float m_Duration;
	private float m_Time;

	private float m_OriginalRange;
	private float m_OriginalIntensity;

	void Awake () {
		m_Light = GetComponent<Light>();

		m_OriginalRange = m_Light.range;
		m_OriginalIntensity = m_Light.intensity;

		m_Duration = 1 / m_FlickeringRate;
	}
	
	void Update () {
		m_Time += Time.deltaTime;
		if(m_Time >= m_Duration) {
			m_Time = 0f;

			m_Light.range = m_OriginalRange * Random.Range(m_FlickeringRange, 1 + m_FlickeringRange);
			m_Light.intensity = m_OriginalIntensity * Random.Range(m_FlickeringRange, 1 + m_FlickeringRange);
		}
	}
}
