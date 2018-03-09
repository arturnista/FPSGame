using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision other) {
		PlayerMovement player = other.transform.GetComponent<PlayerMovement>();
		if(player) {
			Debug.Log("Set position");
		}
	}
}
