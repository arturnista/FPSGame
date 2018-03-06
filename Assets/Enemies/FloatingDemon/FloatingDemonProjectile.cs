using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDemonProjectile : MonoBehaviour {

	private Rigidbody m_Rigidbody;

	[SerializeField]	
	private float m_Damage = 25f;
	[SerializeField]	
	private float m_DamagePush = 15f;
	[SerializeField]
	private float m_Acceleration = 2f;
	private float m_CurrentSpeed;

	void Awake () {
		m_Rigidbody = GetComponent<Rigidbody>();
	}

	void Start () {
		m_CurrentSpeed = 0f;
		m_Rigidbody.velocity = transform.forward * 0f;
	}
	
	// Update is called once per frame
	void Update () {
		m_CurrentSpeed += m_Acceleration * Time.deltaTime;
		m_Rigidbody.velocity = transform.forward * m_CurrentSpeed;
	}

	void OnCollisionEnter(Collision other) {
		PlayerHealth pHealth = other.transform.GetComponent<PlayerHealth>();
		if(pHealth) {
			Vector3 pushDir = Vector3.Normalize( pHealth.transform.position - transform.position );
			pushDir.y = 0f;
			pHealth.TakeDamage(m_Damage, pushDir * m_DamagePush);
		}
		Destroy(this.gameObject);
	}
}
