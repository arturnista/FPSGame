using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollMovement : EnemyMovement {

	protected PlayerMovement m_Player;
	private Animation m_Animation;
	private TrollHealth m_Health;

	private bool m_IsAttacking;
	private bool m_IsTakingHit;
	private bool m_IsDying;

	protected override void Awake () {
		base.Awake();

		m_Animation = GetComponent<Animation>();
		m_Player = GameObject.FindObjectOfType<PlayerMovement>();
		m_Health = GetComponent<TrollHealth>();
	}
	
	void Update () {
		if(m_IsDying) return;

		transform.LookAt(m_Player.transform);
		Vector3 nRot = transform.eulerAngles;
		nRot.x = nRot.z = 0f;
		transform.eulerAngles = nRot;

		if(m_IsAttacking) return;

		if(Vector3.Distance(m_Player.transform.position, transform.position) <= 2f) {
			this.Attack();
			return;
		}

        if (planeVelocity.magnitude > 0.1f) m_Animation.CrossFade("Run");
        else m_Animation.CrossFade("Idle_01");
		m_ForwardSpeed = 1f;

		float speed = this.ComputeSpeed();
		this.Move(speed);
	}

	void Attack() {
		m_IsAttacking = true;
		m_Animation.CrossFade("Attack_01");
		// if(Random.Range(0f, 1f) > .5f) m_Animation.CrossFade("Attack_01");
		// else m_Animation.CrossFade("attack_2");
		Invoke("FinishAttack", 1.15f);		
	}

	void FinishAttack() {
		m_IsAttacking = false;
	}

    public override void TakeDamage(float damage) {
		if(m_IsDying) return;

		m_IsTakingHit = true;
		m_Velocity = Vector3.zero;

		if(m_Health.healthPerc <= 0f) {
			m_IsDying = true;
			m_Animation.CrossFade("Die");
			Invoke("FinishDeath", 2f);
		} else {
			Invoke("FinishTakeDamage", 1f);
			m_Animation.Blend("Hit", .3f, 1f);
		}
    }

	void FinishDeath() {
		Destroy(this.gameObject);
	}

	void FinishTakeDamage() {
		m_IsTakingHit = false;
	}
}
