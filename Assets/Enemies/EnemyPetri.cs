using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPetri : MonoBehaviour {

	public string petriName;

	public void Death () {
		PetriNetController.main.petriNet.AddMarkers(petriName, 1);
		Debug.Log("Death: " + petriName);
	}

}
