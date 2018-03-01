using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopMovement : EnemyMovement {

	protected PlayerHealth m_Player;
	private Animation m_Animation;
	private CyclopHealth m_Health;

	[SerializeField]	
	private float m_StunTime;
	[SerializeField]
	private float m_RunSpeed;
	private float m_WalkSpeed;

	private int m_AttackingType;
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
		bool lookAtPlayer = true;
		bool canMove = true;

		if(!m_IsFollowingPlayer || m_IsStunned || m_IsDying) {
			lookAtPlayer = false;
			canMove = false;
		} else if(m_IsAttacking || (!m_IsAngry && m_IsTakingHit)) {
			canMove = false;
		}

		PlayAnimation();

		if(lookAtPlayer) {
			transform.LookAt(m_Player.transform);
			Vector3 nRot = transform.eulerAngles;
			nRot.x = nRot.z = 0f;
			transform.eulerAngles = nRot;
		}

		if(canMove) {
			if(Vector3.Distance(m_Player.transform.position, transform.position) <= 2f) {
				this.Attack();
				return;
			}

			m_ForwardSpeed = 1f;	
		} else {
			m_ForwardSpeed = 0f;
		}	
		
		float speed = this.ComputeSpeed();
		this.Move(speed);
	}

	void PlayAnimation() {
		if(m_IsDying) {
			m_Animation.CrossFade("death");
			return;
		}
		
		if(m_IsStunned) {
			if(!m_IsTakingHit) {
				m_Animation.CrossFade("stunned_idle");
				return;
			} else {
				m_Animation.CrossFade("stunned_idle_hit");
				return;
			}
		}

		if(!m_IsAngry) {
			if(m_IsTakingHit) {
				m_Animation.CrossFade("hit");
				return;
			}
		}

		if(m_IsAttacking) {
			if(m_AttackingType == 1) {
				m_Animation.CrossFade("attack_1");
				return;
			} else if(m_AttackingType == 2) {
				m_Animation.CrossFade("attack_2");
				return;
			}
		}

        if (planeVelocity.magnitude > 3f) m_Animation.CrossFade("run");
        else if (planeVelocity.magnitude > 0.1f) m_Animation.CrossFade("walk");
		else m_Animation.CrossFade("idle");
	}

	void Attack() {
		m_IsAttacking = true;
		if(Random.Range(0f, 1f) > .5f) {
			m_AttackingType = 1;
			Invoke("HitCheck", m_IsAngry ? .4f : .8f);		
		} else {
			m_AttackingType = 2;
			Invoke("HitCheck", m_IsAngry ? .225f : .45f);		
		}
		
		Invoke("FinishAttack", m_IsAngry ? 1f : 2f);		
	}

	void HitCheck() {
		if(!m_IsAngry && m_IsTakingHit) return;
		if(m_IsDying) return;
		if(m_IsStunned) return;

		if(Vector3.Distance(m_Player.transform.position, transform.position) <= 2f) {
			m_Player.TakeDamage(25f, this);
		}		
	}

	void FinishAttack() {
		m_IsAttacking = false;
	}

    public override void TakeDamage(float damage, string name) {
		if(m_IsDying) return;
		m_IsFollowingPlayer = true;

		m_IsTakingHit = true;

		if(m_Health.healthPerc <= .5f) this.SetAngry();
		
		if(m_Health.healthPerc <= 0f) {
			m_IsDying = true;
			foreach(Collider coll in GetComponentsInChildren<Collider>()) coll.enabled = false;
			Invoke("FinishDeath", 10f);
		} else {
			Invoke("FinishTakeDamage", m_IsAngry ? .5f : 1f);

			if(m_IsAngry) return;
			if(name == "head") {

				m_IsStunned = true;
				Invoke("FinishStunned", m_StunTime);

				this.SetAngry();
			}
		}
    }

	void SetAngry() {
		acceleration *= 2f;
		moveSpeed = m_RunSpeed;
		m_IsAngry = true;
		foreach (AnimationState state in m_Animation) state.speed = 2f;
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
