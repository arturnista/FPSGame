using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public float moveSpeed = 500f;
	public float jumpForce = 500f; 
	public LayerMask groundLayers;
	public float groundCollisionOff = .5f;

	private Transform m_Head;
	private Rigidbody m_Rigidbody;

	private float m_VerticalSpeed;

	public bool m_IsGrounded = true;

	void Awake () {
		m_Head = transform.Find("Head");
		m_Rigidbody = GetComponent<Rigidbody>();
	}
	
	void Update () {
		float m_Forward = Input.GetAxisRaw("Vertical") * Time.deltaTime;
		float m_Sideways = Input.GetAxisRaw("Horizontal") * Time.deltaTime;


		float m_CurrentMoveSpeed = moveSpeed;
		if(m_Forward != 0 && m_Sideways != 0) m_CurrentMoveSpeed *= .7f;
		
		this.CheckGrounded();
		if(m_IsGrounded && Input.GetKeyDown(KeyCode.Space)) {
			m_Rigidbody.AddForce(transform.up * jumpForce);
		}
		// if(m_IsGrounded) {
		// 	m_VerticalSpeed = 0f;
		// } else {
		// 	m_VerticalSpeed += Physics.gravity.y * Time.deltaTime;
		// }
		Vector3 m_Velocity = (transform.forward * m_Forward + transform.right * m_Sideways) * m_CurrentMoveSpeed;
		m_Velocity.y = m_Rigidbody.velocity.y;
		m_Rigidbody.velocity = m_Velocity;
		// transform.Translate(m_Velocity);		
	}

	void CheckGrounded() {
		m_IsGrounded = false;
		RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.down, 10f, groundLayers);

		foreach(RaycastHit hit in hits) {
			if(hit.distance < groundCollisionOff) {
				m_IsGrounded = true;
			}
			// if(m_VerticalSpeed < 0) {
			// 	float nextDistance = hit.distance + ( m_VerticalSpeed * Time.deltaTime );
			// 	if(nextDistance < groundCollisionOff) {
			// 		m_IsGrounded = true;
			// 	}
			// }
		}
	}
}
