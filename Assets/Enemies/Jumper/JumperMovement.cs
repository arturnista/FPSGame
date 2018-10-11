using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperMovement : EnemyMovement {

	protected PlayerMovement m_Player;
	private JumperHealth m_Health;

	private bool m_IsAttacking;
	private bool m_IsTakingHit;
	private bool m_IsDying;

	protected override void Awake () {
		base.Awake();

		m_Player = GameObject.FindObjectOfType<PlayerMovement>();
		m_Health = GetComponent<JumperHealth>();
	}
	
	void Update () {
		if(m_IsDying) return;

		if(m_IsFollowingPlayer) {
			transform.LookAt(m_Player.transform);
			Vector3 nRot = transform.eulerAngles;
			nRot.x = nRot.z = 0f;
			transform.eulerAngles = nRot;

			m_ForwardSpeed = 1f;

			if(m_Controller.isGrounded) {
				m_ForwardSpeed = 1.5f;
				Jump();
			}
		}

		if(m_IsAttacking) {
			m_ForwardSpeed = 0f;
		} else if(Vector3.Distance(m_Player.transform.position, transform.position) <= 2f) {
			this.Attack();
			return;
		}

		float speed = this.ComputeSpeed();
		this.Move(speed);
	}

	void Attack() {
		m_IsAttacking = true;
		// if(Random.Range(0f, 1f) > .5f) m_Animation.CrossFade("Attack_01");
		// else m_Animation.CrossFade("attack_2");
		Invoke("FinishAttack", 1.15f);		
	}

	void FinishAttack() {
		m_IsAttacking = false;
	}

    public override void TakeDamage(float damage, string name) {
		if(m_IsDying) return;

		m_IsFollowingPlayer = true;

		m_IsTakingHit = true;
		m_Velocity = Vector3.zero;

		if(m_Health.healthPerc <= 0f) {
			m_IsDying = true;
			Invoke("FinishDeath", 2f);
		} else {
			Invoke("FinishTakeDamage", 1f);
		}
    }

	void FinishDeath() {
		Destroy(this.gameObject);
	}

	void FinishTakeDamage() {
		m_IsTakingHit = false;
	}
}
