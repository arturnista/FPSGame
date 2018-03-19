using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ItemInside {

	public ItemInside (Transform t) {
		transform = t;
		parent = t.parent;
	}

	public Transform transform;
	public Transform parent;
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

		if(t >= 1f) this.Stop();
	}

	public void Activate() {
		if(m_IsRotating) return;
		m_IsRotating = true;

		m_StartTime = Time.time;
		m_OriginalRotation = transform.eulerAngles.y;
	}

	public void Stop() {
		m_IsRotating = false;
	}

	void OnTriggerEnter(Collider coll) {
		PayloadBehaviour payload = coll.GetComponent<PayloadBehaviour>();
		if(!payload) return;

		m_ObjectsInside.Add(new ItemInside(coll.transform));
		coll.transform.SetParent(transform);
	}

	void OnTriggerExit(Collider coll) {
		ItemInside c = m_ObjectsInside.Find(x => x.transform == coll.transform);
		if(c == null) return;

		c.transform.SetParent(c.parent);
		m_ObjectsInside.Remove(c);
	}
}
