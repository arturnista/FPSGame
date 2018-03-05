using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PhysicEntity {

	public static PlayerMovement Instance;

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
		float speed = this.ComputeSpeed();

		ignoreGravity = false;		
		if(m_StairMode) {
			float cSpeed = Input.GetAxisRaw("Vertical");
			ignoreGravity = true;
			m_DesiredVelocity = cSpeed * speed * m_Head.forward;
		} else if(m_Controller.isGrounded) {
			m_ForwardSpeed = Input.GetAxisRaw("Vertical");
			m_SidewaysSpeed = Input.GetAxisRaw("Horizontal");
		}

		bool lastGrounded = m_Controller.isGrounded;
		float verticalSpeed = m_Velocity.y;

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
		if(m_StairMode) speed *= .6f;
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
		float mSize = m_Controller.radius + m_Controller.skinWidth + .1f;

		m_StairMode = false;
		Collider[] colls = Physics.OverlapBox(transform.position, new Vector3(mSize, .3f, mSize), transform.rotation);
		foreach(Collider c in colls) {
			Stair stair = c.transform.GetComponent<Stair>();
			if(stair) m_StairMode = true;
		}

		if(lastStair && !m_StairMode) {
			m_ForwardSpeed = 1f;
		}
    }

}
