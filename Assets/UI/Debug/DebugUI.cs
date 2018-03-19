using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour {

	private Text m_DebugText;

	private PlayerMovement m_PlayerMovement;

	void Start () {
		m_DebugText = transform.Find("DebugText").GetComponent<Text>();

		m_PlayerMovement = Player.movement;
	}
	
	void Update () {
		m_DebugText.text = "";
		m_DebugText.text += "D: " + m_PlayerMovement.desiredVelocity + " (" + Mathf.Round(m_PlayerMovement.desiredVelocity.magnitude) + ")\n";
		m_DebugText.text += "V: " + m_PlayerMovement.currentVelocity + " (" + Mathf.Round(m_PlayerMovement.currentVelocity.magnitude) + ")\n";
		m_DebugText.text += "P: " + m_PlayerMovement.transform.position + "\n";
		m_DebugText.text += "G: " + (m_PlayerMovement.isGrounded ? "IS GROUNDED" : "IS IN THE AIR") + "\n";
	}
}
