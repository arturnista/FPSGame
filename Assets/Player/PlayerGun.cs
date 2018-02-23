using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour {

	[SerializeField]
	private int m_MagazineSize;
	[SerializeField]
	private int m_MaxAmmo;
	[SerializeField]
	private float m_ReloadTime;
	[SerializeField]
	private float m_Damage;

	[SerializeField]
	private bool m_IsAutomatic;

	[SerializeField]
	private float m_FireRate;
	private float m_FireDelay;

	private int m_CurrentMagazine;

	private Transform m_Head;
	private PlayerMovement m_PlayerMovement;
	private Animator m_Animator;
	private ParticleSystem m_Flash;

	void Awake () {
		m_Flash = GetComponentInChildren<ParticleSystem>();
		m_PlayerMovement = GetComponentInParent<PlayerMovement>();
		m_Animator = GetComponent<Animator>();

		m_FireDelay = 1 / m_FireRate;
		m_CurrentMagazine = m_MagazineSize;
	}

	void Update() {
		// m_Animator.SetInteger("sqrSpeed", Mathf.RoundToInt(m_PlayerMovement.planeVelocity.sqrMagnitude));
		m_Animator.SetBool("isGrounded", m_PlayerMovement.isGrounded);
	}
	
	public void StartShooting (Transform head) {
		m_Head = head;
		if(m_IsAutomatic) InvokeRepeating("Shoot", 0f, m_FireDelay);
		else Shoot();
	}
	
	public void StopShooting () {
		CancelInvoke();
	}

	public void Reload() {
		m_Animator.SetTrigger("reload");
		Invoke("FinishReload", m_ReloadTime);
	}

	void FinishReload() {
		m_CurrentMagazine = m_MagazineSize;
	}

	void Shoot() {
		if(m_CurrentMagazine <= 0) {
			// Play empty sound
			return;
		}

		// Play shot sound
		m_CurrentMagazine--;
		m_Flash.Play();
		m_Animator.SetTrigger("fire");

		Debug.DrawRay(m_Head.transform.position, m_Head.transform.forward * 10f, Color.red, 10f);

		float force = m_Damage;
		RaycastHit[] hits = Physics.RaycastAll(m_Head.transform.position, m_Head.transform.forward);
		foreach(RaycastHit hit in hits) {
			MaterialType material = hit.transform.GetComponent<MaterialType>();
			if(material) {
				force = material.Impact(hit.point, hit.normal, force);
				if(force <= 0f) break;
			}
			// Instantiate(impactEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
		}
	}
}
