using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MaterialType {

	[SerializeField]
	private Material m_OnMaterial;
	[SerializeField]
	private Material m_OffMaterial;

	private bool m_IsOn;
	private bool m_IsBroken;

	private Light m_Light;
	
	void Start () {
		this.Config("metal");
		m_Light = GetComponentInChildren<Light>();
		m_IsOn = true;
	}

	public override float Impact(Vector3 position, Vector3 normal, float force) {
		float newForce = base.Impact(position, normal, force);
		
		m_IsBroken = true;
		this.Off();

		return newForce;
	}

	public void On() {
		if(m_IsBroken || m_IsOn) return;
		m_IsOn = true;

		m_Light.gameObject.SetActive(true);
		Material[] mats = m_Renderer.materials;
		mats[1] = m_OnMaterial;
		m_Renderer.materials = mats;
	}

	public void Off() {
		if(!m_IsOn) return;

		m_IsOn = false;
		m_Light.gameObject.SetActive(false);
		Material[] mats = m_Renderer.materials;
		mats[1] = m_OffMaterial;
		m_Renderer.materials = mats;
	}
}
