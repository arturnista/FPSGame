using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovement : PhysicEntity {

	protected PlayerMovement m_Player;

	protected override void Awake () {
		base.Awake();

		m_Player = GameObject.FindObjectOfType<PlayerMovement>();
	}
	
	void Update () {
		m_ForwardSpeed = .3f;

		Vector3 diff = Vector3.Normalize( m_Player.transform.position - transform.position );
		float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		transform.eulerAngles = new Vector3(0f, angle, 0f);

		float speed = this.ComputeSpeed();
		this.Move(speed);
	}
}
