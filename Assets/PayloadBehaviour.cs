using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadBehaviour : MonoBehaviour {


	public GameObject prefab;
	[SerializeField]
	private float m_MoveSpeed = 5f;

	private bool m_IsMoving;
	private CapsuleCollider m_CapsuleCollider;

	void Awake () {
		m_CapsuleCollider = GetComponent<CapsuleCollider>();
	}
	
	void Update () {
		if(m_IsMoving) {
			transform.Translate(Vector3.up * m_MoveSpeed * Time.deltaTime);
		}
	}

	public void Activate() {
		foreach(Collider coll in Physics.OverlapBox(transform.position + (transform.up * transform.localScale.y), new Vector3(1f, 1f, 1f))) {
			PayloadStopTrigger tStop = coll.GetComponent<PayloadStopTrigger>();
			if(tStop) return;
		}
		m_IsMoving = true;
	}

	public void Stop() {
		m_IsMoving = false;
	}
}
