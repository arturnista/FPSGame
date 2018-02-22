using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovement : EnemyMovement {

	protected PlayerMovement m_Player;

	protected override void Awake () {
		base.Awake();

		m_Player = GameObject.FindObjectOfType<PlayerMovement>();
	}
	
	void Update () {
		m_ForwardSpeed = .3f;

		transform.LookAt(m_Player.transform);
		Vector3 nRot = transform.eulerAngles;
		nRot.x = nRot.z = 0f;
		transform.eulerAngles = nRot;

		float speed = this.ComputeSpeed();
		this.Move(speed);
	}

    public override void TakeDamage() {
		m_Velocity = Vector3.zero;
    }
}
