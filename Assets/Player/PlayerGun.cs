using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour {

	[SerializeField]
	private bool m_IsAutomatic;

	[SerializeField]
	private float m_FireRate;
	private float m_FireDelay;

	private Transform m_Head;
	private ParticleSystem m_Flash;

	void Awake () {
		m_Flash = GetComponentInChildren<ParticleSystem>();

		m_FireDelay = 1 / m_FireRate;
	}
	
	public void StartShoting (Transform head) {
		m_Head = head;
		if(m_IsAutomatic) InvokeRepeating("Shot", 0f, m_FireDelay);
		else Shot();
	}
	
	public void StopShoting () {
		CancelInvoke();
	}

	void Shot() {
		m_Flash.Play();

		// Debug.DrawRay(m_Head.transform.position, m_Head.transform.forward * 10f, Color.red, 10f);

		RaycastHit[] hits = Physics.RaycastAll(m_Head.transform.position, m_Head.transform.forward);
		foreach(RaycastHit hit in hits) {
			MaterialType material = hit.transform.GetComponent<MaterialType>();
			if(material) {
				material.Impact(hit.point, hit.normal);
			}
			// Instantiate(impactEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
		}
	}
}
