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
	private List<Transform> m_Guns;

	private int m_GunToFire;

	protected override void Awake () {
		base.Awake ();

		m_FireDelay = 1 / m_FireRate;
		m_Guns = new List<Transform> ();
		BodyPart[] bodyParts = GetComponentsInChildren<BodyPart> ();
		foreach (BodyPart part in bodyParts) {
			if (part.partName == "gun") m_Guns.Add (part.transform);
		}
	}

	void Update () {

		bool canTurn = true;
		bool canMove = true;
		bool canFire = true;

		if (!m_IsFollowingPlayer) {
			canTurn = false;
			canMove = false;
			canFire = false;
		}

		if (canFire) {
			m_FireTime += Time.deltaTime;
			if (m_FireTime >= m_FireDelay) {
				m_FireTime = 0f;
				Fire ();
				ChangeVariation ();
			}
		}

		if (canTurn) {
			// transform.LookAt (m_Player.transform);
			Vector3 relativePos = m_Player.transform.position - transform.position;
			Quaternion rotationDest = Quaternion.LookRotation (relativePos);
			transform.rotation = Quaternion.RotateTowards (transform.rotation, rotationDest, 100f * Time.deltaTime);

			foreach (Transform gun in m_Guns) {
				Vector3 relativeGunPos = m_Player.transform.position - transform.position;
				Quaternion rotationGunDest = Quaternion.LookRotation (relativeGunPos);
				gun.transform.rotation = Quaternion.RotateTowards (gun.transform.rotation, rotationGunDest, 120f * Time.deltaTime);
			}
		}

		if (canMove) {
			m_ForwardSpeed = 1f;

			float distance = Vector3.Distance (transform.position, m_Player.transform.position);
			if (distance <= m_MinDistance) m_ForwardSpeed = 0f;
		} else {
			m_ForwardSpeed = 0f;
		}
	}

	void FixedUpdate() {
		float speed = this.ComputeSpeed ();
		this.Move (speed);
	}

	public override void TakeDamage (float damage, string name) {
		m_IsFollowingPlayer = true;
	}

	void ChangeVariation () {
		m_SidewaysSpeed = Random.Range (-.5f, .5f);
		if (transform.position.y < m_Player.transform.position.y + 3) {
			m_VerticalSpeed = Random.Range (0, 2f);
		} else {
			m_VerticalSpeed = Random.Range (-2f, 0f);
		}
	}

	void Fire () {
		Vector3 castPosition = transform.position + transform.forward * 2f;
		float distance = Vector3.Distance (castPosition, m_Player.transform.position);
		if (distance >= 30f) return;

		m_GunToFire = 0;

		RaycastHit[] hits = Physics.RaycastAll (castPosition, transform.forward, distance);
		if (hits.Length == 1) {
			for (int x = 0; x < m_Guns.Count; x++) {
				Invoke ("FireProjectile", .5f * x);
			}
		}
	}

	void FireProjectile () {
		Instantiate (projectilePrefab, m_Guns[m_GunToFire].position + m_Guns[m_GunToFire].forward * 2f, Quaternion.Euler (m_Guns[m_GunToFire].eulerAngles));
		m_GunToFire++;
	}
}