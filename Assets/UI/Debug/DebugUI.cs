using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour {

	private Text m_DesiredVelocityText;
	private Text m_VelocityText;

	private PlayerMovement m_PlayerMovement;

	void Awake () {
		m_DesiredVelocityText = transform.Find("DesiredVelocityText").GetComponent<Text>();
		m_VelocityText = transform.Find("VelocityText").GetComponent<Text>();

		m_PlayerMovement = GameObject.FindObjectOfType<PlayerMovement>();
	}
	
	void Update () {
		m_DesiredVelocityText.text = "D: " + m_PlayerMovement.desiredVelocity + "(" + Mathf.Round(m_PlayerMovement.desiredVelocity.magnitude) + ")";
		m_VelocityText.text = "V: " + m_PlayerMovement.currentVelocity + "(" + Mathf.Round(m_PlayerMovement.currentVelocity.magnitude) + ")";
	}
}
