using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    [SerializeField]
    private float m_MaxHealth;
    private float m_CurrentHealth;
    
    void Awake() {
        m_CurrentHealth = m_MaxHealth;
    }

    public void DealDamage(float damage) {
        m_CurrentHealth -= damage;
        if(m_CurrentHealth <= 0f) {
            gameObject.SetActive(false);
        }
    }
}
