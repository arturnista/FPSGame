using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : EnemyHealth {
    
    public override void DealDamage(float damage, string name) {
        m_CurrentHealth -= damage;
        m_EnemyMovement.TakeDamage(damage, name);
    }
}
