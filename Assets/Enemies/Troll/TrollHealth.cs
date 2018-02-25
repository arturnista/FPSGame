using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollHealth : EnemyHealth {
    
    public override void DealDamage(float damage) {
        m_CurrentHealth -= damage;
        m_EnemyMovement.TakeDamage(damage);
    }
}
