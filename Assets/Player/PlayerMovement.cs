using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PhysicEntity {

	protected bool m_IsWalking;
	protected bool m_IsCrouched;
	protected bool m_StairMode;

	private Transform m_Head;
	private PlayerHealth m_Health;

	[Header("Step configuration")]
	[SerializeField]
	private float m_StepSize;
	[SerializeField]
	private AudioClip[] m_StepSounds;
	private AudioSource m_AudioSource;
	private Vector3 m_LastPosition;
	private float m_AmountWalked;

	protected override void Awake () {
		base.Awake();

		m_IsCrouched = false;
		m_Head = transform.Find("Head");
		m_Health = GetComponent<PlayerHealth>();
		m_AudioSource = GetComponent<AudioSource>();
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

		m_AmountWalked += Vector3.Distance(m_LastPosition, transform.position);
		m_LastPosition = transform.position;
		if(m_Controller.isGrounded && m_AmountWalked >= m_StepSize) {
			m_AmountWalked = 0f;
			if(m_StepSounds.Length > 0) {
				SoundController.PlaySound(m_AudioSource, m_StepSounds);
			}
		}
		CollisionFlags coll = this.Move(speed);

		if(!lastGrounded && m_Controller.isGrounded) {
			float floatDamage = Mathf.Lerp(-15f, 100f, Mathf.Abs(verticalSpeed) / 50);
			if(floatDamage > 0f) m_Health.TakeDamage(floatDamage);
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
		if(m_StairMode) speed *= .8f;
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
