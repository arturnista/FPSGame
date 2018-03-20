using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour {
	
	public Vector3 angularSpeed;

	void Start () {
		
	}
	
	void Update () {
		transform.eulerAngles += angularSpeed * Time.deltaTime;
	}
}
