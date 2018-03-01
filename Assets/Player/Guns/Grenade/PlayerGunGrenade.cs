using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunGrenade : PlayerGun {
	
	public GameObject grenadePrefab;

	private float m_HoldTime;
    private bool m_IsCharging;

	public override void StartShooting (Transform head) {
		if(m_IsReloading) return;
		if(Time.time < m_NextShootTime) return;

		m_IsCharging = true;

		m_NextShootTime = Time.time + m_FireDelay;
		m_Head = head;

		m_RecoverSpreadTime = 0f;
		m_IsShooting = true;

		m_HoldTime = Time.time;
	}
	
	public override void StopShooting (bool reload = true) {
		if(m_IsReloading) return;

		if(m_IsCharging) {
			Shoot();
			Reload();
		}
		
		m_IsCharging = false;
	}

	protected override void HitCheck () {
		GrenadeProjectile nade = Instantiate(grenadePrefab, m_Head.position + m_Head.forward, Quaternion.identity).GetComponent<GrenadeProjectile>();
		nade.Throw(m_Head.transform.forward + m_Head.up * .3f, Time.time - m_HoldTime);
	}
}
