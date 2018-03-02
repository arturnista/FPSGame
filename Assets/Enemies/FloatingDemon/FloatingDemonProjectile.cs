using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDemonProjectile : MonoBehaviour {

	private Rigidbody m_Rigidbody;

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
			pHealth.TakeDamage(10f);
		}
		Destroy(this.gameObject);
	}
}
