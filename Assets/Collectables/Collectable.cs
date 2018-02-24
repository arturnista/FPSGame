using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

	protected enum CollectableType {
		Ammo,
		Health,
		Custom
	}

	[SerializeField]
	protected CollectableType m_Type;
	[SerializeField]
	protected int m_Amount;
	[SerializeField]
	protected string m_GunName;

	void Start () {
		
	}
	
	void Update () {
		
	}

	void OnTriggerEnter(Collider coll) {
		PlayerWeapon weapon = coll.GetComponent<PlayerWeapon>();
		if(weapon) {
			int newAmount = weapon.GiveAmmo(m_GunName, m_Amount);
			m_Amount = newAmount;
			if(newAmount == 0) Destroy(this.gameObject);
		}
	}
}
