using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterBehaviour : MonoBehaviour {

	public Vector3 landPosition {
		get {
			return m_LandPosition;
		}
	}

	[SerializeField]
	private Vector3 m_LandPosition;
	private HelicopterFreeSpace m_FreeSpace;

	private float m_MoveSpeed = 10f;

	private bool m_CanMoveToLand = false;
	private bool m_CanLand = false;

	void Awake () {
		m_FreeSpace = GameObject.FindObjectOfType<HelicopterFreeSpace>();
	}
	
	void Update () {
		if(m_CanMoveToLand) this.MoveToPosition();
		if(m_CanLand) this.MoveToLand();
	}

	void MoveToPosition() {
		Vector3 planePos = transform.position;
		planePos.y = 0f;
		Vector3 planeLPos = m_LandPosition;
		planeLPos.y = 0f;
		float distance = Vector3.Distance(planePos, planeLPos);

		transform.LookAt(m_LandPosition);
		Vector3 nRot = transform.eulerAngles;
		nRot.x = nRot.z = 0f;
		nRot.y += 90f;
		transform.eulerAngles = nRot;
		transform.position += -transform.right * m_MoveSpeed * Time.deltaTime * Mathf.Clamp01(distance / 10f);

		if(distance <= 0.1f) {
			m_CanLand = true;
		}
	}

	void MoveToLand() {
		float distance = Vector3.Distance(transform.position, m_LandPosition);
		
		transform.position -= transform.up * m_MoveSpeed / 2f * Time.deltaTime * Mathf.Clamp01(distance / 10f);;

		if(distance <= 0.1f) {
			m_CanLand = false;
			m_CanMoveToLand = false;

			m_FreeSpace.Explode();
		}
	}

	public void Land() {
		m_CanMoveToLand = true;
	}
}
