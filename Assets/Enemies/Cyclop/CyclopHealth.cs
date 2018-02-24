using UnityEngine;

public class CyclopHealth : EnemyHealth {
    
    public override void DealDamage(float damage) {
        m_CurrentHealth -= damage;
        m_EnemyMovement.TakeDamage(damage);
    }
}
