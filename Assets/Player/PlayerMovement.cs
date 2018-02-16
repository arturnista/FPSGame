using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public float acceleration = 100f;
	public float moveSpeed = 500f;
	public float jumpForce = 500f; 
	public LayerMask groundLayers;
	public float groundCollisionOff = .5f;

	public Vector3 currentVelocity {
		get {
			return m_Velocity;
		}
	}

	public Vector3 desiredVelocity {
		get {
			return m_DesiredVelocity;
		}
	}

	private Transform m_Head;
	private Rigidbody m_Rigidbody;

	private float m_VerticalSpeed;
	private float m_ForwardSpeed;
	private float m_SidewaysSpeed;

	private Vector3 m_ForwardDirection;
	private Vector3 m_SidewaysDirection;

	private Vector3 m_Velocity;
	private Vector3 m_DesiredVelocity;
	
	public bool m_IsGrounded = true;

	private Vector3 m_LastVelocity;

	void Awake () {
		m_Head = transform.Find("Head");
		m_Rigidbody = GetComponent<Rigidbody>();
	}
	
	void Update () {
		bool lastGround = m_IsGrounded;
		m_IsGrounded = this.CheckGrounded();

		if(m_IsGrounded) {
			if(!lastGround) {
				m_ForwardSpeed = 0f;
				m_SidewaysSpeed = 0f;
				m_Velocity = Vector3.zero;
			} else {
				m_ForwardSpeed = Input.GetAxisRaw("Vertical") * Time.deltaTime;
				m_SidewaysSpeed = Input.GetAxisRaw("Horizontal") * Time.deltaTime;
			}

			m_ForwardDirection = transform.forward;
			m_SidewaysDirection = transform.right;
		} else {
			float fowardAirSpeed = Input.GetAxisRaw("Vertical") * Time.deltaTime;
			if(fowardAirSpeed < 0f) {
				float oSign = Mathf.Sign(m_ForwardSpeed);
				m_ForwardSpeed = Mathf.Clamp(Mathf.Abs(m_ForwardSpeed) - (acceleration * .2f * Time.deltaTime), 0f, m_ForwardSpeed);
				m_ForwardSpeed *= oSign;
			}
		}

		float m_CurrentMoveSpeed = moveSpeed;
		if(m_ForwardSpeed != 0 && m_SidewaysSpeed != 0) m_CurrentMoveSpeed *= .7f;

		if(m_IsGrounded && Input.GetKeyDown(KeyCode.Space)) {
			m_Rigidbody.AddForce(transform.up * jumpForce);
		}

		m_DesiredVelocity = (m_ForwardDirection * m_ForwardSpeed + m_SidewaysDirection * m_SidewaysSpeed) * m_CurrentMoveSpeed;

		float deltaX = Mathf.Abs(m_DesiredVelocity.x - m_Velocity.x) / Mathf.Abs(m_DesiredVelocity.z - m_Velocity.z);
		float deltaY = 1;
		float deltaSum = deltaX + deltaY;

		deltaX = deltaX / deltaSum;
		deltaY = deltaY / deltaSum;

		float acelX = acceleration * Time.deltaTime * deltaX;
		if(m_Velocity.x < m_DesiredVelocity.x) m_Velocity.x = Mathf.Clamp(m_Velocity.x + acelX, m_Velocity.x, m_DesiredVelocity.x);
		else if(m_Velocity.x > m_DesiredVelocity.x) m_Velocity.x = Mathf.Clamp(m_Velocity.x - acelX, m_DesiredVelocity.x, m_Velocity.x);

		float acelY = acceleration * Time.deltaTime * deltaY;
		if(m_Velocity.z < m_DesiredVelocity.z) m_Velocity.z = Mathf.Clamp(m_Velocity.z + acelY, m_Velocity.z, m_DesiredVelocity.z);
		else if(m_Velocity.z > m_DesiredVelocity.z) m_Velocity.z = Mathf.Clamp(m_Velocity.z - acelY, m_DesiredVelocity.z, m_Velocity.z);

		m_Velocity.y = m_Rigidbody.velocity.y;
		m_Rigidbody.velocity = m_Velocity;
		m_LastVelocity = m_Velocity;
		// transform.Translate(m_Velocity);		
	}

	void LateUpdate() {
		Vector3 diff = m_LastVelocity - m_Rigidbody.velocity;
		// Debug.Log(diff);
	}

	bool CheckGrounded() {
		RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.down, 5f, groundLayers);

		foreach(RaycastHit hit in hits) {
			if(hit.distance < groundCollisionOff) {
				return true;
			}
		}
		return false;
	}
}
