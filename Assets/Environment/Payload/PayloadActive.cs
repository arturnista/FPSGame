using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadActive : DoorActivate {

	public string petriName;
	private PayloadBehaviour m_PayloadBehaviour;

	void Start () {
		m_PayloadBehaviour = GameObject.FindObjectOfType<PayloadBehaviour>();
	}
	
	public override void Use () {
		PetriNetController.main.petriNet.AddMarkers(petriName, 1);
		m_PayloadBehaviour.Activate();
	}
}
