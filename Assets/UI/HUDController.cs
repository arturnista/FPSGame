using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

	private Text m_AmmoText;
	private Text m_HealthText;
	private PlayerWeapon m_PlayerWeapon;
	private PlayerHealth m_PlayerHealth;

	void Awake () {
		m_AmmoText = transform.Find("AmmoText").GetComponent<Text>();
		m_HealthText = transform.Find("HealthText").GetComponent<Text>();
		m_PlayerWeapon = GameObject.FindObjectOfType<PlayerWeapon>();
		m_PlayerHealth = GameObject.FindObjectOfType<PlayerHealth>();
	}
	
	void Update () {
		m_AmmoText.text = m_PlayerWeapon.magazine + " / " + m_PlayerWeapon.ammo;
		m_HealthText.text = Mathf.RoundToInt(m_PlayerHealth.health).ToString();
	}
}
