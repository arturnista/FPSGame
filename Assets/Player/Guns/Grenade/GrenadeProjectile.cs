using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour {

    public GameObject explosionEffect;

    [SerializeField]
    private float m_ExplosionTime;
    [SerializeField]
    private float m_ThrowForce;
    [SerializeField]
    private float m_Damage;
    [SerializeField]
    private float m_Radius;

    private Rigidbody m_Rigidbody;

    private float m_DurationTime;

    public void Throw(Vector3 dir, float holdTime) {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.velocity = dir * m_ThrowForce + PlayerMovement.Instance.currentVelocity;

        m_DurationTime = 0f;
    }

    void Update() {
        m_DurationTime += Time.deltaTime;
        if(m_DurationTime >= m_ExplosionTime) {
            Explode();
        }
    }

    void Explode() {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Collider[] colls = Physics.OverlapSphere(transform.position, m_Radius);
        foreach(Collider c in colls) {
            EnemyHealth en = c.GetComponent<EnemyHealth>();
            if(en) {
                float dmg = m_Damage * 1 / Vector3.Distance(transform.position, en.transform.position);
                en.DealDamage(dmg, "body");
            } else {
                PlayerHealth pl = c.GetComponent<PlayerHealth>();
                if(pl) {
                    float dmg = m_Damage * 1 / Vector3.Distance(transform.position, pl.transform.position);
                    Vector3 pushDir = Vector3.Normalize( pl.transform.position - transform.position );
                    pl.TakeDamage(dmg, pushDir * dmg);
                }
            }
        }
        Destroy(this.gameObject);
    }

}
