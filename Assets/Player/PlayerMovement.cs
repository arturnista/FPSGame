using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PhysicEntity {

	public static PlayerMovement Instance;

	[SerializeField]
	protected float m_StairSpeed = 3f;

	protected bool m_IsWalking;
	protected bool m_IsCrouched;
	protected bool m_StairMode;

	private Transform m_Head;
	private PlayerHealth m_Health;


	protected override void Awake () {
		base.Awake();

		Instance = this;

		m_IsCrouched = false;
		m_Head = transform.Find("Head");
		m_Health = GetComponent<PlayerHealth>();
	}

	void Update() {
		if(m_StairMode) {
			m_Velocity.y = Input.GetAxisRaw("Vertical") * m_StairSpeed * m_Head.forward.y;
			m_ForwardSpeed = 0f;
			m_SidewaysSpeed = Input.GetAxisRaw("Horizontal");
		} else if(m_Controller.isGrounded) {
			m_ForwardSpeed = Input.GetAxisRaw("Vertical");
			m_SidewaysSpeed = Input.GetAxisRaw("Horizontal");
		}

		bool lastGrounded = m_Controller.isGrounded;
		float verticalSpeed = m_Velocity.y;

		float speed = this.ComputeSpeed();
		CollisionFlags coll = this.Move(speed);

		if(!lastGrounded && m_Controller.isGrounded) {
			float floatDamage = Mathf.Abs(verticalSpeed);
			if(floatDamage >= 10f) m_Health.TakeDamage(floatDamage * 3);
		}

		if(m_Controller.isGrounded && Input.GetKeyDown(KeyCode.Space)) {
			if(m_StairMode) m_Velocity = m_Velocity + transform.forward * -10f; 
			else Jump();
			
		}

		if(Input.GetKeyDown(KeyCode.LeftControl)) {
			Crouch();
		} else if(Input.GetKeyUp(KeyCode.LeftControl)) {
			UnCrouch();
		}

		if(Input.GetKeyDown(KeyCode.LeftShift)) {
			m_IsWalking = true;
		} else if(Input.GetKeyUp(KeyCode.LeftShift)) {
			m_IsWalking = false;
		}

		CheckStair();
	}

	protected override float ComputeSpeed() {
		float speed = base.ComputeSpeed();

		if(m_IsWalking || m_IsCrouched) speed *= .4f;
		return speed;
	}

	protected void Crouch() {
		m_Controller.height = height / 2f;
		m_IsCrouched = true;
	}

	protected void UnCrouch() {
		m_Controller.height = height;
		m_IsCrouched = false;
	}

    void CheckStair() {
		bool lastStair = m_StairMode;

		m_StairMode = false;
		Collider[] colls = Physics.OverlapBox(transform.position, new Vector3(1f, .3f, 1f), transform.rotation);
		foreach(Collider c in colls) {
			Stair stair = c.transform.GetComponent<Stair>();
			if(stair) m_StairMode = true;
		}

		if(lastStair && !m_StairMode) {
			m_ForwardSpeed = 1f;
		}
    }

}
