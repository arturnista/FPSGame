using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	private Transform m_Frame;

	private bool m_IsOpen;

	void Awake () {
		m_Frame = transform.Find("DoorFrame");
		m_IsOpen = false;
	}
	
	void Update () {
		
	}

	public void Toggle() {
		if(m_IsOpen) {
			this.Close();
		} else {
			this.Open();
		}
	}

	public void Open() {
		if(m_IsOpen) return;
		m_IsOpen = true;
		m_Frame.gameObject.SetActive(false);
	}

	public void Close() {
		if(!m_IsOpen) return;
		m_IsOpen = false;
		m_Frame.gameObject.SetActive(true);		
	}
}
