using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CyclopMovement : EnemyMovement {
	
	private Animation m_Animation;
	private CyclopHealth m_Health;
	private UnityEngine.AI.NavMeshAgent m_NavMeshAgent;

	[Header("Cyclop")]
	[SerializeField]	
	private float m_Damage = 25f;
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

	[Header("Sounds")]
	[SerializeField]
	private AudioClip[] m_NoticePlayerSounds;
	[SerializeField]
	private AudioClip[] m_AngrySounds;
	[SerializeField]
	private AudioClip[] m_DamageSounds;

	protected override void Awake () {
		base.Awake();

		m_Animation = GetComponent<Animation>();
		m_Health = GetComponent<CyclopHealth>();
		m_NavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

		m_WalkSpeed = moveSpeed;
	}
	
	void Update () {
		if(m_IsDying) {
			return;
		}

		bool canRotate = true;
		bool canMove = true;

		if(!m_IsFollowingPlayer || m_IsStunned) {
			canRotate = false;
			canMove = false;
		} else if(m_IsAttacking || (!m_IsAngry && m_IsTakingHit)) {
			canMove = false;
		}

		PlayAnimation();

		if(canMove) {
			if(Vector3.Distance(m_Player.transform.position, transform.position) <= 2f) {
				this.Attack();
				return;
			}

			m_ForwardSpeed = 1f;	
			m_NavMeshAgent.isStopped = false;
			NavMeshPath path = new NavMeshPath();
			m_NavMeshAgent.CalculatePath(m_Player.transform.position, path);
			if(path.status == NavMeshPathStatus.PathComplete && path.corners.Length > 1) {
				Vector3 target = path.corners[1];
				
				if(canRotate) {
					transform.LookAt(target);
					Vector3 nRot = transform.eulerAngles;
					nRot.x = nRot.z = 0f;
					transform.eulerAngles = nRot;
				}

				float speed = this.ComputeSpeed();
				this.Move(speed);
			}
		} else {
			m_ForwardSpeed = 0f;
			m_NavMeshAgent.isStopped = true;
		}	
	}

	void PlayAnimation() {
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
			Invoke("HitCheck", m_IsAngry ? .45f : .9f);		
			Invoke("FinishAttack", m_IsAngry ? .8f : 1.6f);		
		} else {
			m_AttackingType = 2;
			Invoke("HitCheck", m_IsAngry ? .35f : .7f);		
			Invoke("FinishAttack", m_IsAngry ? .75f : 1.5f);		
		}
		
	}

	void HitCheck() {
		if(!m_IsAngry && m_IsTakingHit) return;
		if(m_IsDying) return;
		if(m_IsStunned) return;

		if(Vector3.Distance(m_Player.transform.position, transform.position) <= 3f) {
			m_Player.TakeDamage(m_Damage);
		}		
	}

	void FinishAttack() {
		m_IsAttacking = false;
	}

	public override void NoticePlayer() {
		base.NoticePlayer();
		SoundController.PlaySound(m_AudioSource, m_NoticePlayerSounds);
	}

    public override void TakeDamage(float damage, string name) {
		if(m_IsDying) return;
		SoundController.PlaySound(m_AudioSource, m_DamageSounds);
		m_IsFollowingPlayer = true;

		m_IsTakingHit = true;

		if(m_Health.healthPerc <= .5f) this.SetAngry();
		
		if(m_Health.healthPerc <= 0f) {
			m_IsDying = true;
			m_Animation.CrossFade("death");
			foreach(Collider coll in GetComponentsInChildren<Collider>()) coll.enabled = false;
			Invoke("FinishDeath", 10f);
		} else {
			Invoke("FinishTakeDamage", m_IsAngry ? .35f : .7f);

			if(m_IsAngry) return;
			if(name == "head") {

				m_IsStunned = true;
				Invoke("FinishStunned", m_StunTime);

				this.SetAngry();
			}
		}
    }

	void SetAngry() {
		if(m_IsAngry) return;
		
		acceleration *= 2f;
		moveSpeed = m_RunSpeed;
		m_IsAngry = true;
		foreach (AnimationState state in m_Animation) state.speed = 2f;
		SoundController.PlaySound(m_AudioSource, m_AngrySounds);
		
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
