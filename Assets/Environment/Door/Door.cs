using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	private Transform m_Frame;
	private Animator m_Animator;

	private bool m_IsOpen;

	public bool isOpen {
		get {
			return m_IsOpen;
		}
	}

	void Awake () {
		m_Frame = transform.Find("DoorFrame");
		m_Animator = GetComponent<Animator>();
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
		if(m_Animator) m_Animator.SetBool("isOpen", m_IsOpen);
		else m_Frame.gameObject.SetActive(false);
	}

	public void Close() {
		if(!m_IsOpen) return;
		m_IsOpen = false;
		if(m_Animator) m_Animator.SetBool("isOpen", m_IsOpen);
		else m_Frame.gameObject.SetActive(true);		
	}
}
