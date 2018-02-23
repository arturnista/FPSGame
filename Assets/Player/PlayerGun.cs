using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerGunSpread {
	public int bullet;
	public Vector2 position;
}

public class PlayerGun : MonoBehaviour {

	public int currentMagazine {
		get {
			return m_CurrentMagazine;
		}
	}
	public int maxMagazine {
		get {
			return m_MaxAmmo;
		}
	}

	[SerializeField]
	protected int m_MagazineSize;
	[SerializeField]
	protected int m_MaxAmmo;
	[SerializeField]
	protected float m_ReloadTime;
	[SerializeField]
	protected float m_Damage;

	[SerializeField]
	protected bool m_IsAutomatic;

	[SerializeField]
	protected float m_FireRate;
	protected float m_FireDelay;

	[Header("Spread")]
	[SerializeField]
	protected float m_DefaultSpread = .1f;
	[SerializeField]
	protected float m_RecoverSpreadDelay = .2f;
	protected float m_RecoverSpreadTime;
	protected int m_CurrentSpreadBullet;
	[SerializeField]
	protected List<PlayerGunSpread> m_SpreadList;
	protected int m_CurrentSpread;

	protected Vector3 m_OriginalPosition;
	protected Vector3 m_OriginalEuler;

	protected int m_CurrentMagazine;
	protected int m_PlayerSpeed;

	protected float m_NextShootTime;

	protected bool m_IsShooting;
	protected bool m_IsReloading;

	protected Transform m_Head;
	protected PlayerMovement m_PlayerMovement;
	protected Animator m_Animator;
	protected ParticleSystem m_Flash;

	void Awake () {
		m_Flash = GetComponentInChildren<ParticleSystem>();
		m_PlayerMovement = GetComponentInParent<PlayerMovement>();
		m_Animator = GetComponent<Animator>();

		m_FireDelay = 1 / m_FireRate;
		m_CurrentMagazine = m_MagazineSize;

		m_CurrentSpread = 0;
		m_CurrentSpreadBullet = 0;

		m_OriginalPosition = transform.localPosition;
		m_OriginalEuler = transform.localEulerAngles;
	}

	void Update() {
		m_PlayerSpeed = Mathf.RoundToInt(m_PlayerMovement.planeVelocity.sqrMagnitude);
		m_Animator.SetInteger("sqrSpeed", m_PlayerSpeed);
		m_Animator.SetBool("isGrounded", m_PlayerMovement.isGrounded);

		if(!m_IsShooting && m_CurrentSpreadBullet > 0) {
			m_RecoverSpreadTime += Time.deltaTime;
			if(m_RecoverSpreadTime > m_RecoverSpreadDelay) {
				m_RecoverSpreadTime = 0f;
				m_CurrentSpreadBullet--;
				if(m_CurrentSpread > 0 && m_SpreadList[m_CurrentSpread].bullet > m_CurrentSpreadBullet) m_CurrentSpread--;
			}
		}
		transform.localEulerAngles = m_OriginalEuler;
		transform.localPosition = m_OriginalPosition;
	}
	
	public virtual void StartShooting (Transform head) {
		if(m_IsReloading) return;
		if(Time.time < m_NextShootTime) return;

		m_NextShootTime = Time.time + m_FireDelay;
		m_Head = head;

		m_RecoverSpreadTime = 0f;
		m_IsShooting = true;

		if(m_IsAutomatic) InvokeRepeating("Shoot", 0f, m_FireDelay);
		else Shoot();
	}
	
	public virtual void StopShooting (bool reload = true) {
		if(m_IsReloading) return;
		m_RecoverSpreadTime = 0f;
		m_IsShooting = false;

		if(m_IsAutomatic) CancelInvoke();
		if(reload && m_CurrentMagazine <= 0) Reload();
	}

	public virtual void Reload() {
		if(m_CurrentMagazine >= m_MagazineSize) {
			return;
		}

		m_IsReloading = true;
		this.StopShooting(false);

		m_Animator.SetTrigger("reload");
		Invoke("FinishReload", m_ReloadTime);
	}

	public virtual void Select() {
		transform.localPosition = m_OriginalPosition;
		transform.localEulerAngles = m_OriginalEuler;
	}

	public virtual void Deselect() {
		this.StopShooting();
		m_IsReloading = false;
		CancelInvoke();
	}

	protected virtual void FinishReload() {
		m_IsReloading = false;
		m_CurrentMagazine = m_MagazineSize;
		m_CurrentSpread = 0;
		m_CurrentSpreadBullet = 0;
	}

	protected virtual void Shoot() {
		if(m_IsReloading) return;
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

		Vector2 lastPos = m_CurrentSpread > 0 ? m_SpreadList[m_CurrentSpread - 1].position : Vector2.zero;
		// Vector3 spreadOffset = Vector3.Slerp(lastPos, cSpread.position, cBullet / (cSpread.bullet - cBulletOff));
		Vector3 spreadOffset = Vector3.Slerp(lastPos, cSpread.position, cBullet / (cSpread.bullet - cBulletOff));

		// Play shot sound
		m_CurrentMagazine--;
		m_CurrentSpreadBullet++;
		m_Flash.Play();
		m_Animator.SetTrigger("fire");

		// Debug.DrawRay(m_Head.transform.position, m_Head.transform.forward * 10f, Color.red, 10f);

		float force = m_Damage;
		Vector3 dir = new Vector3(
			Random.Range(-m_DefaultSpread, m_DefaultSpread) + m_Head.transform.forward.x - (Mathf.Sign(m_Head.transform.forward.x) * m_Head.transform.forward.z * spreadOffset.x), 
			Random.Range(-m_DefaultSpread, m_DefaultSpread) + m_Head.transform.forward.y + spreadOffset.y, 
			Random.Range(-m_DefaultSpread, m_DefaultSpread) + m_Head.transform.forward.z - (Mathf.Sign(m_Head.transform.forward.z) * m_Head.transform.forward.x * spreadOffset.x)
		);
		RaycastHit[] hits = Physics.RaycastAll(m_Head.transform.position, dir);
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
