using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPetri : Door {

	public string petriName;

	// Use this for initialization
	void Start () {
		PetriNetController.main.petriNet.AddListener(petriName, () => {
			this.Open();
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
