using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerGunSpread {
	public int bullet;
	public Vector3 position;
}

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

	[Header("Spread")]
	[SerializeField]
	private float m_RecoverSpreadDelay = .2f;
	private float m_RecoverSpreadTime;
	private int m_CurrentSpreadBullet;
	[SerializeField]
	private List<PlayerGunSpread> m_SpreadList;
	private int m_CurrentSpread;

	private int m_CurrentMagazine;

	private bool m_IsShooting;

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

		m_CurrentSpread = 0;
		m_CurrentSpreadBullet = 0;
	}

	void Update() {
		m_Animator.SetInteger("sqrSpeed", Mathf.RoundToInt(m_PlayerMovement.planeVelocity.sqrMagnitude));
		m_Animator.SetBool("isGrounded", m_PlayerMovement.isGrounded);

		if(!m_IsShooting && m_CurrentSpreadBullet > 0) {
			m_RecoverSpreadTime += Time.deltaTime;
			if(m_RecoverSpreadTime > m_RecoverSpreadDelay) {
				m_RecoverSpreadTime = 0f;
				m_CurrentSpreadBullet--;
				if(m_CurrentSpread > 0 && m_SpreadList[m_CurrentSpread].bullet > m_CurrentSpreadBullet) m_CurrentSpread--;
			}
		}
	}
	
	public void StartShooting (Transform head) {
		m_Head = head;

		m_RecoverSpreadTime = 0f;
		m_IsShooting = true;

		if(m_IsAutomatic) InvokeRepeating("Shoot", 0f, m_FireDelay);
		else Shoot();
	}
	
	public void StopShooting () {
		m_RecoverSpreadTime = 0f;
		m_IsShooting = false;

		CancelInvoke();
	}

	public void Reload() {
		m_Animator.SetTrigger("reload");
		Invoke("FinishReload", m_ReloadTime);
	}

	void FinishReload() {
		m_CurrentMagazine = m_MagazineSize;
		m_CurrentSpread = 0;
		m_CurrentSpreadBullet = 0;
	}

	void Shoot() {
		if(m_CurrentMagazine <= 0) {
			// Play empty sound
			return;
		}

		float cBullet = m_CurrentSpreadBullet;
		PlayerGunSpread cSpread = m_SpreadList[m_CurrentSpread];
		if(cSpread.bullet < cBullet) {
			m_CurrentSpread++;
			cSpread = m_SpreadList[m_CurrentSpread];
		}

		float cBulletOff = m_CurrentSpread > 0 ? m_SpreadList[m_CurrentSpread - 1].bullet : 0;
		cBullet -= cBulletOff;

		Vector3 lastPos = m_CurrentSpread > 0 ? m_SpreadList[m_CurrentSpread - 1].position : Vector3.zero;
		Vector3 spreadOffset = Vector3.Slerp(lastPos, cSpread.position, cBullet / (cSpread.bullet - cBulletOff));

		// Play shot sound
		m_CurrentMagazine--;
		m_CurrentSpreadBullet++;
		m_Flash.Play();
		m_Animator.SetTrigger("fire");

		// Debug.DrawRay(m_Head.transform.position, m_Head.transform.forward * 10f, Color.red, 10f);

		float force = m_Damage;
		RaycastHit[] hits = Physics.RaycastAll(m_Head.transform.position, m_Head.transform.forward + spreadOffset);
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
