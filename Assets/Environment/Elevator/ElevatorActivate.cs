using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorActivate : Interactive {

	private Elevator m_Elevator;

	void Awake () {
		m_Elevator = GetComponentInParent<Elevator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void Use() {
		m_Elevator.Activate();
	}
}
