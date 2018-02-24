using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    [SerializeField]
    protected float m_MaxHealth;
    protected float m_CurrentHealth;

    protected EnemyMovement m_EnemyMovement;

    public float healthPerc {
        get {
            return m_CurrentHealth / m_MaxHealth;
        }
    }
    
    void Awake() {
        m_CurrentHealth = m_MaxHealth;
        m_EnemyMovement = GetComponent<EnemyMovement>();
    }

    public virtual void DealDamage(float damage) {
        m_CurrentHealth -= damage;
        m_EnemyMovement.TakeDamage(damage);
        
        if(m_CurrentHealth <= 0f) {
            gameObject.SetActive(false);
        }
    }
}
