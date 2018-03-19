using UnityEngine;

public class BodyPart : MonoBehaviour {

    public string partName {
        get {
            return m_BodyPartName;
        }
    }

    [SerializeField]
    private string m_BodyPartName;
    [SerializeField]
    private float m_DamageMultiplier = 1f;
    private EnemyHealth m_EnemyHealth;
    
    void Awake() {
        m_EnemyHealth = GetComponentInParent<EnemyHealth>();
    }

    public void DealDamage(float dmg) {
        m_EnemyHealth.DealDamage(dmg * m_DamageMultiplier, m_BodyPartName);
    }
}
