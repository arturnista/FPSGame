using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PhysicEntity {

	protected bool m_IsWalking;
	protected bool m_IsCrouched;

	private Transform m_Head;
	private PlayerHealth m_Health;

	protected override void Awake () {
		base.Awake();

		m_IsCrouched = false;
		m_Head = transform.Find("Head");
		m_Health = GetComponent<PlayerHealth>();
	}

	void Update() {
		m_ForwardSpeed = Input.GetAxisRaw("Vertical");
		m_SidewaysSpeed = Input.GetAxisRaw("Horizontal");

		bool lastGrounded = m_Controller.isGrounded;
		float verticalSpeed = m_Velocity.y;

		float speed = this.ComputeSpeed();
		this.Move(speed);

		if(!lastGrounded && m_Controller.isGrounded) {
			float floatDamage = Mathf.Abs(verticalSpeed);
			if(floatDamage >= 10f) m_Health.TakeDamage(floatDamage * 3);
		}

		if(m_Controller.isGrounded && Input.GetKeyDown(KeyCode.Space)) {
			Jump();
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

}
