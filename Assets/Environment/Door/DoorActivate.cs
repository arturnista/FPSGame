using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorActivate : Interactive {

	public Door door;

	void Awake () {
		if(!door) door = GetComponentInParent<Door>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void Use() {
		base.Use();
		door.Toggle();
	}
}
