using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMovement : EnemyMovement {
	
	private BossHealth m_Health;
	private Animator m_Animator;
	private UnityEngine.AI.NavMeshAgent m_NavMeshAgent;

	[Header("Boss")]
	[SerializeField]	
	private float m_DamageRange = 10f;
	[SerializeField]	
	private float m_Damage = 25f;

	private int m_AttackingType;
	private bool m_IsAttacking;
	private bool m_IsTakingHit;
	private bool m_IsDying;

	[Header("Sounds")]
	[SerializeField]
	private AudioClip[] m_NoticePlayerSounds;
	[SerializeField]
	private AudioClip[] m_DamageSounds;

	protected override void Awake () {
		base.Awake();

		m_IsFollowingPlayer = true;

		m_Health = GetComponent<BossHealth>();
		m_Animator = GetComponent<Animator>();
		m_NavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

	}
	
	void Update () {
		if(m_IsDying) {
			return;
		}

		bool canRotate = true;
		bool canMove = true;

		if(!m_IsFollowingPlayer) {
			canRotate = false;
			canMove = false;
		} else if(m_IsAttacking || m_IsTakingHit) {
			canMove = false;
		}

		PlayAnimation();

		if(canMove) {
			if(Vector3.Distance(m_Player.transform.position, transform.position) <= m_DamageRange) {
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
        if (planeVelocity.magnitude > .01f) m_Animator.SetBool("IsWalking", true);
		else m_Animator.SetBool("IsWalking", false);
	}

	void Attack() {
		m_IsAttacking = true;
		if(Random.Range(0f, 1f) > .5f) {
			m_AttackingType = 1;
			Invoke("HitCheck", .4f);		
			Invoke("FinishAttack", .8f);	
			m_Animator.SetTrigger("Attack1");
		} else {
			m_AttackingType = 2;
			Invoke("HitCheck", .27f);		
			Invoke("HitCheck", 1f);		
			Invoke("FinishAttack", 1.37f);		
			m_Animator.SetTrigger("Attack2");	
		}
		
	}

	void HitCheck() {
		if(m_IsTakingHit) return;
		if(m_IsDying) return;

		if(Vector3.Distance(m_Player.transform.position, transform.position) <= m_DamageRange * 1.3f) {
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

		if(m_Health.healthPerc <= 0f) {
			m_IsDying = true;
			foreach(Collider coll in GetComponentsInChildren<Collider>()) coll.enabled = false;
			m_Animator.SetTrigger("Die");
			Invoke("FinishDeath", 10f);
		} else {
			if(Random.Range(0f, 1f) > .8f) {
				m_Animator.SetTrigger("GetHit");
				m_IsTakingHit = true;
				Invoke("FinishTakeDamage", .57f);
			}
		}
    }

	void FinishDeath() {
		Destroy(this.gameObject);
	}

	void FinishTakeDamage() {
		m_IsTakingHit = false;
	}
}
