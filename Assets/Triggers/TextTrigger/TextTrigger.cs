using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextTrigger : MonoBehaviour {

	public string text;

	private TextMeshPro m_Text;
	private Player m_Player;

	void Awake () {
		m_Text = GetComponentInChildren<TextMeshPro>();
		m_Text.text = text;
		m_Text.gameObject.SetActive(false);
	}

	void Update() {
		if(m_Text.gameObject.active) {
			m_Text.transform.LookAt(Player.look.FPSCamera.transform);
		}
	}

	void OnTriggerEnter(Collider other) {
		Player pl = other.GetComponent<Player>();
		if(pl) {
			m_Player = pl;
			m_Text.gameObject.SetActive(true);
		}
	}

	void OnTriggerExit(Collider other) {
		Player pl = other.GetComponent<Player>();
		if(pl) {
			m_Player = null;
			m_Text.gameObject.SetActive(false);
		}
	}

}
