using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterBehaviour : MonoBehaviour {

	[SerializeField]
	private float m_CurrentSpeed;

	private float m_AngleToMove = 25f;

	void Awake () {
		
	}
	
	void Update () {
		this.Move();
	}

	void Move() {
		if(m_CurrentSpeed <= 0f) return;

		// transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 25f);
		transform.position += -transform.right * m_CurrentSpeed * Time.deltaTime;
	}
}
