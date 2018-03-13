using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

	protected enum CollectableType {
		Ammo,
		Gun,
		Health,
		Custom
	}

	[SerializeField]
	protected CollectableType m_Type;
	[SerializeField]
	protected int m_Amount;
	[SerializeField]
	protected string m_GunName;

	[SerializeField]
	protected AudioClip m_PickUpAudio;

	void Awake () {

	}
	
	void Update () {
		
	}

	void OnTriggerEnter(Collider coll) {
		if(m_Type == CollectableType.Ammo) {
			PlayerWeapon weapon = coll.GetComponent<PlayerWeapon>();
			if(weapon) {
				int newAmount = weapon.GiveAmmo(m_GunName, m_Amount);
				m_Amount = newAmount;
				if(newAmount == 0) Destroy(this.gameObject);
				PickUp(m_GunName);
			}
		} else if(m_Type == CollectableType.Gun) {
			PlayerWeapon weapon = coll.GetComponent<PlayerWeapon>();
			if(weapon) {
				weapon.GiveGun(m_GunName, m_Amount);
				Destroy(this.gameObject);
				PickUp(m_GunName);
			}
		} else if(m_Type == CollectableType.Health) {
			PlayerHealth health = coll.GetComponent<PlayerHealth>();
			if(health) {
				health.GiveHealth(m_Amount);
				Destroy(this.gameObject);
				PickUp("health");
			}
		}
	}

	void PickUp(string itemName) {
		SoundController.PlaySound(Player.audioSource, m_PickUpAudio);
		HUDController.main.PickUpItem(itemName, m_Amount);
	}
}
