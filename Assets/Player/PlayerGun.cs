using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerGunSpread {
	public int bullet;
	public Vector2 position;
}

public class PlayerGun : MonoBehaviour {

	public int currentAmmo {
		get {
			return m_CurrentAmmo;
		}
	}
	public int currentMagazine {
		get {
			return m_CurrentMagazine;
		}
	}
	public string gunName {
		get {
			return m_GunName;
		}
	}
	public bool isAvailable {
		get {
			return m_IsAvailable;
		}
	}

	[SerializeField]
	private string m_GunName;
	[SerializeField]
	protected bool m_IsAvailable;
	[SerializeField]
	protected int m_InitialAmmo = 0;

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

	[SerializeField]
	[RangeAttribute(0f, 1f)]
	protected float m_Penetration = 1f;

	[Header("Sound")]
	protected AudioSource m_AudioSource;
	[SerializeField]
	protected AudioClip[] m_ShotAudios;
	[SerializeField]
	protected AudioClip m_EmptyAudio;
	[SerializeField]
	protected AudioClip m_ReloadAudio;
	[SerializeField]
	protected AudioClip m_SelectAudio;

	[Header("Spread")]
	[SerializeField]
	protected float m_InitialSpread = 0f;
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
	protected int m_CurrentAmmo;
	protected int m_PlayerSpeed;

	protected float m_NextShootTime;

	protected bool m_IsShooting;
	protected bool m_IsReloading;

	protected Transform m_Head;
	protected PlayerMovement m_PlayerMovement;
	protected Animator m_Animator;
	[SerializeField]
	protected ParticleSystem m_Flash;
	[SerializeField]
	protected ParticleSystem m_Cartridge;

	protected bool m_HasAwaken;

	void Awake () {
		m_HasAwaken = true;

		m_PlayerMovement = GetComponentInParent<PlayerMovement>();
		m_Animator = GetComponent<Animator>();
		m_AudioSource = GetComponentInParent<AudioSource>();
		Renderer renderer = GetComponentInChildren<Renderer>();
		renderer.sortingOrder = 5000;

		m_FireDelay = 1 / m_FireRate;
		m_CurrentAmmo = m_InitialAmmo;
		m_CurrentMagazine = 0;

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

	public virtual int GiveGun(int amount) {
		m_IsAvailable = true;
		return GiveAmmo(amount);
	}

	public virtual int GiveAmmo(int amount) {
		if(!m_HasAwaken) m_InitialAmmo += amount;

		m_CurrentAmmo += amount;
		if(m_CurrentAmmo > m_MaxAmmo) {
			int diff = m_CurrentAmmo - m_MaxAmmo;
			m_CurrentAmmo = m_MaxAmmo;
			return diff;
		}
		return 0;
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
		if(m_IsReloading) return;
		if(m_CurrentMagazine >= m_MagazineSize || m_CurrentAmmo == 0) {
			return;
		}

		m_IsReloading = true;
		this.StopShooting(false);
		
		if(m_ReloadAudio) SoundController.PlaySound(m_AudioSource, m_ReloadAudio);

		m_Animator.SetTrigger("reload");
		Invoke("FinishReload", m_ReloadTime);
	}

	public virtual void Select() {
		if(m_SelectAudio) SoundController.PlaySound(m_AudioSource, m_SelectAudio);
		if(m_CurrentMagazine == 0) this.Reload();

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
		if(m_CurrentAmmo > m_MagazineSize) {
			int ammoToReload = m_MagazineSize - m_CurrentMagazine;
			m_CurrentMagazine = m_MagazineSize;
			m_CurrentAmmo -= ammoToReload;
		} else {
			m_CurrentMagazine = m_CurrentAmmo;
			m_CurrentAmmo = 0;
		}
		m_CurrentSpread = 0;
		m_CurrentSpreadBullet = 0;
	}

	protected virtual void Shoot() {
		if(m_IsReloading) return;
		if(m_CurrentMagazine <= 0) {
			if(m_EmptyAudio) SoundController.PlaySound(m_AudioSource, m_EmptyAudio);
			return;
		}

		// Play shot sound
		if(m_ShotAudios.Length > 0) {
			SoundController.PlaySound(m_AudioSource, m_ShotAudios);
		}
		m_CurrentMagazine--;
		m_CurrentSpreadBullet++;
		if(m_Flash) m_Flash.Play();
		if(m_Cartridge) m_Cartridge.Play();
		m_Animator.SetTrigger("fire");

		this.HitCheck();
	}

	protected virtual void HitCheck() {
		Debug.DrawRay(m_Head.transform.position, m_Head.transform.forward * 10f, Color.red, 10f);

		float force = m_Damage;
		RaycastHit[] hits = Physics.RaycastAll(m_Head.transform.position, Spread());
		
		List<RaycastHit> hitsList = new List<RaycastHit>(hits);
		hitsList.Sort((p1,p2) => Mathf.RoundToInt( p1.distance - p2.distance ));

		foreach(RaycastHit hit in hitsList) {
			MaterialType material = hit.transform.GetComponent<MaterialType>();
			if(material) {
				force = material.Impact(hit.point, hit.normal, force) * m_Penetration;
				if(force <= 1f) break;
			}
		}
	}

	private Vector3 Spread() {
		if(m_SpreadList.Count == 0) {
			return AmountBasedSpread();
		} else {
			return RecoilBasedSpread();
		}
	}

	private Vector3 AmountBasedSpread() {
		float cSpreadRange = (m_DefaultSpread * m_CurrentSpreadBullet / m_MagazineSize) + m_InitialSpread;
		return new Vector3(
			Random.Range(-cSpreadRange, cSpreadRange) + m_Head.transform.forward.x, // - (Mathf.Sign(m_Head.transform.forward.x) * m_Head.transform.forward.z * spreadOffset.x), 
			Random.Range(-cSpreadRange, cSpreadRange) + m_Head.transform.forward.y, // + spreadOffset.y, 
			Random.Range(-cSpreadRange, cSpreadRange) + m_Head.transform.forward.z // - (Mathf.Sign(m_Head.transform.forward.z) * m_Head.transform.forward.x * spreadOffset.x)
		);
	}

	private Vector3 RecoilBasedSpread() {
		float cBullet = m_CurrentSpreadBullet;
		PlayerGunSpread cSpread = m_SpreadList[m_CurrentSpread];
		if(cSpread.bullet < cBullet) {
			m_CurrentSpread++;
			cSpread = m_SpreadList[m_CurrentSpread];
		}

		float cBulletOff = m_CurrentSpread > 0 ? m_SpreadList[m_CurrentSpread - 1].bullet : 0;
		cBullet -= cBulletOff;

		Vector2 lastPos = m_CurrentSpread > 0 ? m_SpreadList[m_CurrentSpread - 1].position : Vector2.zero;
		Vector3 spreadOffset = Vector3.Slerp(lastPos, cSpread.position, cBullet / (cSpread.bullet - cBulletOff));

		return new Vector3(
			Random.Range(-m_DefaultSpread, m_DefaultSpread) + m_Head.transform.forward.x - (Mathf.Sign(m_Head.transform.forward.x) * m_Head.transform.forward.z * spreadOffset.x), 
			Random.Range(-m_DefaultSpread, m_DefaultSpread) + m_Head.transform.forward.y + spreadOffset.y, 
			Random.Range(-m_DefaultSpread, m_DefaultSpread) + m_Head.transform.forward.z - (Mathf.Sign(m_Head.transform.forward.z) * m_Head.transform.forward.x * spreadOffset.x)
		);
	}
}
