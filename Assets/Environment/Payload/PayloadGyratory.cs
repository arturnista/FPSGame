using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ItemInside {

	public ItemInside (Transform t) {
		transform = t;
		SetOriginalRotation();
	}
	public void SetOriginalRotation() {
		originalRotation = transform.eulerAngles.y; 		
	}

	public float originalRotation;
	public Transform transform;
}

public class PayloadGyratory : MonoBehaviour {
	List<ItemInside> m_ObjectsInside;

	private float m_OriginalRotation;
	private float m_Duration = 10f;
	private float m_StartTime;

	private bool m_IsRotating;

	void Awake () {
		m_ObjectsInside = new List<ItemInside>();
		m_IsRotating = false;
	}
	
	void Update () {
		Rotate();
	}

	void Rotate() {
		if(!m_IsRotating) return;
		
		float t = (Time.time - m_StartTime) / m_Duration;

		Vector3 rotation = transform.eulerAngles;
		rotation.y = Mathf.Lerp(m_OriginalRotation, m_OriginalRotation + 90f, t);
		transform.eulerAngles = rotation;

		foreach(ItemInside c in m_ObjectsInside) {
			Vector3 cRot = c.transform.eulerAngles;
			cRot.y = Mathf.Lerp(c.originalRotation, c.originalRotation + 90f, t);
			c.transform.eulerAngles = cRot;
		}

		if(t >= 1f) m_IsRotating = false;
	}

	public void Activate() {
		if(m_IsRotating) return;
		m_IsRotating = true;

		m_StartTime = Time.time;
		m_OriginalRotation = transform.eulerAngles.y;
		foreach(ItemInside c in m_ObjectsInside) c.SetOriginalRotation();
	}

	public void Stop() {

	}

	void OnTriggerEnter(Collider coll) {
		m_ObjectsInside.Add(new ItemInside(coll.transform));
	}

	void OnTriggerExit(Collider coll) {
		ItemInside c = m_ObjectsInside.Find(x => x.transform == coll.transform);
		m_ObjectsInside.Remove(c);
	}
}
