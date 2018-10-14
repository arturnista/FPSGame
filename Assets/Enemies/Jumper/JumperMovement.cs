using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JumperMovement : EnemyMovement {

	[Header("Jumper")]
	[SerializeField]	
	private float m_Damage = 10f;

	private JumperHealth m_Health;
	private UnityEngine.AI.NavMeshAgent m_NavMeshAgent;

	private bool m_IsAttacking;
	private bool m_IsTakingHit;
	private bool m_IsDying;

	protected override void Awake () {
		base.Awake();

		m_Health = GetComponent<JumperHealth>();
		m_NavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}
	
	void Update () {
		if(m_IsDying) return;

		float playerDist = Vector3.Distance(m_Player.transform.position, transform.position);

		if(!m_IsAttacking && m_IsFollowingPlayer) {
			// NavMeshPath path = new NavMeshPath();
			// m_NavMeshAgent.CalculatePath(m_Player.transform.position, path);
			// if(path.status == NavMeshPathStatus.PathComplete && path.corners.Length > 1) {
			// 	Vector3 target = path.corners[1];
				
			// 	transform.LookAt(target);
			// 	Vector3 nRot = transform.eulerAngles;
			// 	nRot.x = nRot.z = 0f;
			// 	transform.eulerAngles = nRot;
			// }
			
			transform.LookAt(m_Player.transform);
			Vector3 nRot = transform.eulerAngles;
			nRot.x = nRot.z = 0f;
			transform.eulerAngles = nRot;

			m_ForwardSpeed = 1f;

			if(playerDist <= 2f) {
				Attack();
			} else if(playerDist <= 10f) {
				m_ForwardSpeed = 5f;

				if(m_Controller.isGrounded) {
					Jump();
				}
			}
		}

		float speed = this.ComputeSpeed();
		this.Move(speed);
	}

	void Attack() {
		m_IsAttacking = true;

		m_Player.TakeDamage(m_Damage);
		Invoke("FinishAttack", 2f);
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
