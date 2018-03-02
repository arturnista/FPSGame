using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDemonMovement : EnemyMovement {
	
	public GameObject projectilePrefab;
	[SerializeField]
	private float m_MinDistance = 20f;
	private float m_FireRate = 1f;
	private float m_FireDelay = 1f;

	private float m_FireTime = 0f;

	protected override void Awake () {
		base.Awake();

		m_FireDelay = 1 / m_FireRate;
	}

	void Update () {

		bool canTurn = true;
		bool canMove = true;
		bool canFire = true;

		if(!m_IsFollowingPlayer) {
			canTurn = false;
			canMove = false;
			canFire = false;
		}

		if(canFire) {
			m_FireTime += Time.deltaTime;
			if(m_FireTime >= m_FireDelay) {
				m_FireTime = 0f;
				Fire();
			}
		}

		if(canTurn) {
			transform.LookAt(m_Player.transform);
		}

		if(canMove) {
			m_ForwardSpeed = 1f;	

			float distance = Vector3.Distance(transform.position, m_Player.transform.position);
			if(distance <= m_MinDistance) m_ForwardSpeed = 0f;
			
			if(transform.position.y < m_Player.transform.position.y + 3) {
				m_DesiredVelocity.y = 4f;
			} else {
				m_DesiredVelocity.y = 0f;
			}
		} else {
			m_ForwardSpeed = 0f;
		}
		
		float speed = this.ComputeSpeed();
		this.Move(speed);
	}

	void Fire() {
		Instantiate(projectilePrefab, transform.position + transform.forward * 2f, Quaternion.Euler(transform.eulerAngles));
	}
}
