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
	protected int m_CurrentAmount;
	[SerializeField]
	protected string m_GunName;

	[SerializeField]
	protected AudioClip m_PickUpAudio;

	void Awake () {
		m_CurrentAmount = m_Amount;
	}
	
	void Update () {
		
	}

	void OnTriggerEnter(Collider coll) {
		if(m_Type == CollectableType.Ammo) {
			PlayerWeapon weapon = coll.GetComponent<PlayerWeapon>();
			if(weapon) {
				int newAmount = weapon.GiveAmmo(m_GunName, m_CurrentAmount);
				int pickUpAmount = m_CurrentAmount - newAmount;
				m_CurrentAmount = newAmount;
				if(newAmount == 0) Destroy(this.gameObject);
				PickUp(m_GunName, pickUpAmount);
			}
		} else if(m_Type == CollectableType.Gun) {
			PlayerWeapon weapon = coll.GetComponent<PlayerWeapon>();
			if(weapon) {
				weapon.GiveGun(m_GunName, m_CurrentAmount);
				Destroy(this.gameObject);
				PickUp(m_GunName, m_CurrentAmount);
			}
		} else if(m_Type == CollectableType.Health) {
			PlayerHealth health = coll.GetComponent<PlayerHealth>();
			if(health) {
				if(health.GiveHealth(m_CurrentAmount)) {
					Destroy(this.gameObject);
					PickUp("health", m_CurrentAmount);
				}
			}
		}
	}

	void PickUp(string itemName, int amount) {
		Debug.Log("ITEM_PICKUP|"+itemName+"|"+amount);

		SoundController.PlaySound(Player.audioSource, m_PickUpAudio);
		HUDController.main.PickUpItem(itemName, amount);
	}
}
