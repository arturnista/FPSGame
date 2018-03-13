using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneMaterial : MaterialType {

	void Start () {
		this.Config("stone");
	}

	public override float Impact(Vector3 position, Vector3 normal, float force) {
		float newForce = base.Impact(position, normal, force);
		// Debug.Log("Impact on Glass with " + force + "(" + newForce + ")" + " on " + normal);
		return newForce;
	}

}
