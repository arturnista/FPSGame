using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSpan : MonoBehaviour {

	[SerializeField]
	public float m_Duration;

	void Awake() {
		Invoke("DestroyObject", m_Duration);
	}

	void DestroyObject () {
		Destroy(this.gameObject);
	}
}
