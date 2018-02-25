using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopMovement : EnemyMovement {

	protected PlayerHealth m_Player;
	private Animation m_Animation;
	private CyclopHealth m_Health;
	
	[SerializeField]
	private float m_RunSpeed;
	private float m_WalkSpeed;

	private bool m_IsAttacking;
	private bool m_IsTakingHit;
	private bool m_IsStunned;
	private bool m_IsDying;
	private bool m_IsAngry;

	protected override void Awake () {
		base.Awake();

		m_Animation = GetComponent<Animation>();
		m_Player = GameObject.FindObjectOfType<PlayerHealth>();
		m_Health = GetComponent<CyclopHealth>();

		m_WalkSpeed = moveSpeed;
	}
	
	void Update () {
		if(m_IsDying || m_IsStunned) return;

		transform.LookAt(m_Player.transform);
		Vector3 nRot = transform.eulerAngles;
		nRot.x = nRot.z = 0f;
		transform.eulerAngles = nRot;

		if(m_IsAttacking || (!m_IsAngry && m_IsTakingHit)) return;

		if(Vector3.Distance(m_Player.transform.position, transform.position) <= 2f) {
			this.Attack();
			return;
		}

        if (planeVelocity.magnitude > 2f) m_Animation.CrossFade("run");
        else if (planeVelocity.magnitude > 0.1f) m_Animation.CrossFade("walk");
        else if (m_IsStunned) m_Animation.CrossFade("stunned_idle");
        else m_Animation.CrossFade("idle");
		m_ForwardSpeed = 1f;

		float speed = this.ComputeSpeed();
		this.Move(speed);
	}

	void Attack() {
		m_IsAttacking = true;
		if(Random.Range(0f, 1f) > .5f) {
			m_Animation.CrossFade("attack_1");
			Invoke("HitCheck", m_IsAngry ? .4f : .8f);		
		} else {
			m_Animation.CrossFade("attack_2");
			Invoke("HitCheck", m_IsAngry ? .225f : .45f);		
		}
		
		Invoke("FinishAttack", m_IsAngry ? 1f : 2f);		
	}

	void HitCheck() {
		if(Vector3.Distance(m_Player.transform.position, transform.position) <= 2f) {
			m_Player.TakeDamage(25f, this);
		}		
	}

	void FinishAttack() {
		m_IsAttacking = false;
	}

    public override void TakeDamage(float damage) {
		if(m_IsDying) return;

		m_IsTakingHit = true;
		m_Velocity = Vector3.zero;

		if(m_Health.healthPerc <= .5f) {
			acceleration *= 2f;
			moveSpeed = m_RunSpeed;
			m_IsAngry = true;
			foreach (AnimationState state in m_Animation) state.speed = 2f;
		}
		
		if(m_Health.healthPerc <= 0f) {
			m_IsDying = true;
			m_Animation.CrossFade("death");
			Invoke("FinishDeath", 10f);
		} else {
			if(m_IsAngry) return;

			Invoke("FinishTakeDamage", m_IsAngry ? .5f : 1f);
			if(m_IsStunned) m_Animation.CrossFade("stunned_idle_hit");
			else m_Animation.CrossFade("hit");
			if(!m_IsAngry && damage >= 14f) {
				m_IsStunned = true;
				m_Animation.Blend("stunned_idle", 1f, 1f);
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
