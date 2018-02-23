using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

	private Text m_AmmoText;
	private PlayerWeapon m_PlayerWeapon;

	void Awake () {
		m_AmmoText = transform.Find("AmmoText").GetComponent<Text>();
		m_PlayerWeapon = GameObject.FindObjectOfType<PlayerWeapon>();
	}
	
	void Update () {
		m_AmmoText.text = m_PlayerWeapon.ammo + " / " + m_PlayerWeapon.maxAmmo;
	}
}
