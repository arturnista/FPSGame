using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopMovement : EnemyMovement {

	protected PlayerMovement m_Player;
	private Animation m_Animation;
	private CyclopHealth m_Health;

	private bool m_IsAttacking;
	private bool m_IsTakingHit;
	private bool m_IsStunned;
	private bool m_IsDying;

	protected override void Awake () {
		base.Awake();

		m_Animation = GetComponent<Animation>();
		m_Player = GameObject.FindObjectOfType<PlayerMovement>();
		m_Health = GetComponent<CyclopHealth>();
	}
	
	void Update () {
		if(m_IsDying || m_IsStunned) return;

		transform.LookAt(m_Player.transform);
		Vector3 nRot = transform.eulerAngles;
		nRot.x = nRot.z = 0f;
		transform.eulerAngles = nRot;

		if(m_IsAttacking || m_IsTakingHit) return;

		if(Vector3.Distance(m_Player.transform.position, transform.position) <= 2f) {
			this.Attack();
			return;
		}

        if (planeVelocity.magnitude > 6f) m_Animation.CrossFade("run");
        else if (planeVelocity.magnitude > 0.1f) m_Animation.CrossFade("walk");
        else m_Animation.CrossFade("idle");
		m_ForwardSpeed = 1f;

		float speed = this.ComputeSpeed();
		this.Move(speed);
	}

	void Attack() {
		m_IsAttacking = true;
		if(Random.Range(0f, 1f) > .5f) m_Animation.CrossFade("attack_1");
		else m_Animation.CrossFade("attack_2");
		Invoke("FinishAttack", 2f);		
	}

	void FinishAttack() {
		m_IsAttacking = false;
	}

    public override void TakeDamage(float damage) {
		if(m_IsDying) return;

		m_IsTakingHit = true;
		m_Velocity = Vector3.zero;

		if(m_Health.healthPerc <= .5f) moveSpeed = 8f;
		
		if(m_Health.healthPerc <= 0f) {
			m_IsDying = true;
			m_Animation.CrossFade("death");
			Invoke("FinishDeath", 10f);
		} else {
			Invoke("FinishTakeDamage", 1f);
			if(m_IsStunned) m_Animation.CrossFade("stunned_idle_hit");
			else m_Animation.CrossFade("hit");
			Debug.Log(damage);
			if(damage >= 14f) {
				m_IsStunned = true;
				m_Animation.CrossFade("stunned_idle", 2f);
				Invoke("FinishStunned", 10f);
			}
		}
    }

	void FinishStunned() {
		m_IsStunned = false;
	}

	void FinishDeath() {
		Destroy(this.gameObject);
	}

	void FinishTakeDamage() {
		m_IsTakingHit = false;
	}
}
