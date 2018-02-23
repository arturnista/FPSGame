using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour {
	
	private Transform m_Head;
	private PlayerGun m_Gun;

	public int ammo {
		get {
			return m_Gun.currentMagazine;
		}
	}
	public int maxAmmo {
		get {
			return m_Gun.maxMagazine;
		}
	}

	private int m_CurrentWeapon;

	void Awake () {
		m_Head = transform.Find("Head");
	}

	void Start() {
		SelectWeapon(0, true);		
	}
	
	void Update () {
		if(Input.GetMouseButtonDown(0)) {
			m_Gun.StartShooting(m_Head);
		} else if(Input.GetMouseButtonUp(0)) {
			m_Gun.StopShooting();
		}

		if(Input.GetKeyDown(KeyCode.R)) {
			m_Gun.Reload();
		}

		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			SelectWeapon(0);
		} else if(Input.GetKeyDown(KeyCode.Alpha2)) {
			SelectWeapon(1);
		} else if(Input.GetKeyDown(KeyCode.Alpha3)) {
			SelectWeapon(2);
		}
	}

	void SelectWeapon(int index, bool force = false) {
		if(!force && m_CurrentWeapon == index) return;

		if(m_Gun) m_Gun.Deselect();

		m_Head.GetChild(m_CurrentWeapon).gameObject.SetActive(false);
		m_CurrentWeapon = index;
		m_Head.GetChild(m_CurrentWeapon).gameObject.SetActive(true);

		m_Gun = m_Head.GetChild(m_CurrentWeapon).GetComponent<PlayerGun>();
		m_Gun.Select();
	}
}
