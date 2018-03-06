using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorActivate : Interactive {

	private Door m_Door;

	void Awake () {
		m_Door = GetComponentInParent<Door>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void Use() {
		base.Use();
		m_Door.Toggle();
	}
}
