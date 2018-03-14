using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

	public static HUDController main;

	public GameObject pickupFeedItem;

	private Text m_AmmoText;
	private Text m_HealthText;
	private Image m_HitIndicator;
	private Transform m_PickupFeed;

	[SerializeField]
	private Color m_HitColor;
	private Color m_HitColorTransparent;
	private PlayerWeapon m_PlayerWeapon;
	private PlayerHealth m_PlayerHealth;

	private bool m_IsDead = false;

	private bool m_TakeHit = false;
	private float m_HitDuration = .5f;
	private float m_HitStartTime;

	void Awake () {
		main = this;

		m_AmmoText = transform.Find("AmmoContainer/AmmoText").GetComponent<Text>();
		m_HealthText = transform.Find("HealthContainer/HealthText").GetComponent<Text>();
		
		m_HitIndicator = transform.Find("HitIndicator").GetComponent<Image>();
		m_HitColorTransparent = new Color(1f, 0f, 0f, 0f);

		m_PickupFeed = transform.Find("PickupFeed");
	}

	void Start() {
		m_PlayerWeapon = Player.weapon;
		m_PlayerHealth = Player.health;
	}
	
	void Update () {
		if(m_IsDead) return;
		
		m_AmmoText.text = m_PlayerWeapon.magazine + " / " + m_PlayerWeapon.ammo;
		m_HealthText.text = Mathf.RoundToInt(m_PlayerHealth.health).ToString();

		if(m_TakeHit) {
			float t = (Time.time - m_HitStartTime) / m_HitDuration;
			m_HitIndicator.color = Color.Lerp(m_HitColor, m_HitColorTransparent, t);
			if(t >= 1) m_TakeHit = false;
		}
	}

	public void TakeDamage() {
		m_TakeHit = true;

		Color col = m_HitColor;
		m_HitIndicator.color = col;
		m_HitStartTime = Time.time;
	}

	public void Die() {
		m_IsDead = true;

		m_AmmoText.text = "";
		m_HealthText.text = "";

		Color col = m_HitColor;
		col.a = 1f;
		m_HitIndicator.color = col;
	}

	public void PickUpItem(string itemName, int amount) {
		PickupFeedItem it = Instantiate(pickupFeedItem, transform.position, Quaternion.identity).GetComponent<PickupFeedItem>();
		it.transform.SetParent(m_PickupFeed);
		it.Configure(itemName, amount);
	}
}
