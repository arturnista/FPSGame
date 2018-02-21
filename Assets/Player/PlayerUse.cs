using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUse : MonoBehaviour {
	
	private Transform m_Head;

	void Awake () {
		m_Head = transform.Find("Head");
		
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.E)) {
				RaycastHit hit;
				Debug.DrawRay(m_Head.transform.position, m_Head.transform.forward * 2f, Color.green, 10f);

				if(Physics.Raycast(m_Head.transform.position, m_Head.transform.forward, out hit, 2f)) {
					Interactive interactive = hit.transform.GetComponent<Interactive>();
					if(interactive != null) {
						interactive.Use();
					}
				}

		}
	}
}
