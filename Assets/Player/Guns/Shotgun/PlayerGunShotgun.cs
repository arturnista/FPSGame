using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunShotgun : PlayerGun {
    
    [Header("Shotgun")]
    [SerializeField]
    private int m_ShellsCount;

    protected override void Shoot() {
        if(m_CurrentMagazine <= 0) return;

        int incShell = m_ShellsCount - 1;
		m_CurrentMagazine += incShell;
		m_CurrentSpreadBullet -= incShell;

        for (int i = 0; i < m_ShellsCount; i++) base.Shoot();
    }

	protected override void FinishReload() {
		m_IsReloading = false;

        m_CurrentAmmo--;
		m_CurrentMagazine++;
		m_CurrentSpread = 0;
		m_CurrentSpreadBullet = 0;
        
        if(m_CurrentAmmo == 0) return;

        if(m_CurrentMagazine >= m_MagazineSize) {
            m_CurrentMagazine = m_MagazineSize;
            return;
        }

		m_Animator.SetTrigger("reload");
		Invoke("FinishReload", m_ReloadTime);
	}
}
