using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDemonMovement : EnemyMovement {
	
	public GameObject projectilePrefab;
	[SerializeField]
	private float m_MinDistance = 20f;
	private float m_FireRate = .3f;
	private float m_FireDelay;

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
				ChangeVariation();
			}
		}

		if(canTurn) {
			transform.LookAt(m_Player.transform);
		}

		if(canMove) {
			m_ForwardSpeed = 1f;	

			float distance = Vector3.Distance(transform.position, m_Player.transform.position);
			if(distance <= m_MinDistance) m_ForwardSpeed = 0f;
		} else {
			m_ForwardSpeed = 0f;
		}
		
		float speed = this.ComputeSpeed();
		this.Move(speed);
	}

	void ChangeVariation() {
		m_SidewaysSpeed = Random.Range(-.5f, .5f);
		if(transform.position.y < m_Player.transform.position.y + 3) {
			m_VerticalSpeed = Random.Range(0, 2f);
		} else {
			m_VerticalSpeed = Random.Range(-2f, 0f);
		}
	}

	void Fire() {
		Vector3 castPosition = transform.position + transform.forward * 2f;
		float distance = Vector3.Distance(castPosition, m_Player.transform.position);
		if(distance >= 30f) return;
		
		RaycastHit[] hits = Physics.RaycastAll(castPosition, transform.forward, distance);
		if(hits.Length == 1) {
			for(int x = 0; x < 3; x++) {
				Invoke("FireProjectile", .3f * x);
			}
		}
	}

	void FireProjectile() {
		Instantiate(projectilePrefab, transform.position + transform.forward * 2f, Quaternion.Euler(transform.eulerAngles));
	}
}
