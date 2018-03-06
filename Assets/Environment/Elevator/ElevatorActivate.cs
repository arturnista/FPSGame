using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorActivate : Interactive {

	private Door m_Door;
	public Elevator elevator;
	[SerializeField]
	private bool m_IsInitialActive;

	void Awake () {
		m_Door = GetComponentInParent<Door>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void Use() {
		base.Use();
		if(elevator.isInitialPos == m_IsInitialActive) {
			if(!m_Door.isOpen) m_Door.Open();
			else elevator.Activate();
		} else {
			elevator.Activate();
		}
	}
	
}
