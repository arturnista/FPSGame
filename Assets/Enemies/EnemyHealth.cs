using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    [SerializeField]
    private float m_MaxHealth;
    private float m_CurrentHealth;

    private EnemyMovement m_EnemyMovement;
    
    void Awake() {
        m_CurrentHealth = m_MaxHealth;
        m_EnemyMovement = GetComponent<EnemyMovement>();
    }

    public void DealDamage(float damage) {
        m_CurrentHealth -= damage;
        m_EnemyMovement.TakeDamage();
        
        if(m_CurrentHealth <= 0f) {
            gameObject.SetActive(false);
        }
    }
}
