using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterFreeSpaceStartTrigger : MonoBehaviour {

	private HelicopterFreeSpace m_FreeSpace;

	void Awake() {
		m_FreeSpace = GameObject.FindObjectOfType<HelicopterFreeSpace>();
	}

	void OnTriggerEnter(Collider other) {
		m_FreeSpace.StartWaiting();
	}
}
