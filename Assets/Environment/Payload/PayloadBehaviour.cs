using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadBehaviour : MonoBehaviour {

	[SerializeField]
	private float m_MoveSpeed = 5f;

	private bool m_IsMoving;
	private CapsuleCollider m_CapsuleCollider;

	void Awake () {
		m_CapsuleCollider = GetComponent<CapsuleCollider>();
	}
	
	void Update () {
		if(m_IsMoving && CheckRail()) {
			transform.Translate(Vector3.up * m_MoveSpeed * Time.deltaTime);
		}
	}

	bool CheckRail() {
		float horizontalSize = transform.lossyScale.y;
		float verticalSize = transform.lossyScale.z;
		RaycastHit[] hits = Physics.RaycastAll(transform.position + transform.up * horizontalSize, transform.forward, verticalSize);
		// Debug.DrawRay(transform.position + transform.up * horizontalSize, transform.forward * verticalSize, Color.red, 2f);

		foreach(RaycastHit h in hits) {
			if(h.transform.tag == "Rail") return true;
		}

		m_IsMoving = false;
		return false;
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
