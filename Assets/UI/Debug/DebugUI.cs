using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour {

	private Canvas m_DebugCanvas;
	private Text m_DebugText;

	private PlayerMovement m_PlayerMovement;
	private bool m_IsShowing;

	void Start () {
		m_DebugText = transform.Find("DebugText").GetComponent<Text>();
		m_DebugCanvas = GetComponent<Canvas>();

		m_PlayerMovement = Player.movement;
		m_IsShowing = false;
		m_DebugCanvas.enabled = false;
	}
	
	void Update () {
		if(m_IsShowing) {
			m_DebugText.text = "";
			m_DebugText.text += "D: " + m_PlayerMovement.desiredVelocity + " (" + Mathf.Round(m_PlayerMovement.desiredVelocity.magnitude) + ")\n";
			m_DebugText.text += "V: " + m_PlayerMovement.currentVelocity + " (" + Mathf.Round(m_PlayerMovement.currentVelocity.magnitude) + ")\n";
			m_DebugText.text += "P: " + m_PlayerMovement.transform.position + "\n";
			m_DebugText.text += "G: " + (m_PlayerMovement.isGrounded ? "IS GROUNDED" : "IS IN THE AIR") + "\n";
		}

		if(Input.GetKeyDown(KeyCode.L)) ToggleActive();
	}

	void ToggleActive() {
		m_IsShowing = !m_IsShowing;
		m_DebugCanvas.enabled = m_IsShowing;
		// m_DebugCanvas.gameObject.SetActive(m_IsShowing);
	}
}
