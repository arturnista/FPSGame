using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterFreeSpace : MonoBehaviour {

	public GameObject explosionPrefab;
	public GameObject boss;

	private int m_EnemiesInSpace;
	private bool m_AlreadyNotify = false;

	private float m_TimeToCheck;
	private BoxCollider m_BoxCollider;
	private HelicopterBehaviour m_Helicopter;
	private HUDController m_HUDController;

	void Start () {
		m_BoxCollider = GetComponent<BoxCollider>();
		m_Helicopter = GameObject.FindObjectOfType<HelicopterBehaviour>();
		m_HUDController = GameObject.FindObjectOfType<HUDController>();

		PetriNetController.main.petriNet.AddListener("heli_available", () => {
			Debug.Log("Heli available");
			m_Helicopter.Land();
		});
	}
	
	void Update () {
	}

	public void Explode() {
		Instantiate(explosionPrefab, m_Helicopter.landPosition, Quaternion.identity);
		Destroy(m_Helicopter.gameObject);
		boss.SetActive(true);
	}

	public void StartWaiting() {
		if(m_AlreadyNotify) return;
		m_AlreadyNotify = true;
		m_HUDController.ShowText("Clean the field so the helicopter can land!");
	}

}
