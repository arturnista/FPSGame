using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassMaterial : MaterialType {

	void Start () {
		this.Config("glass");
	}

	public override float Impact(Vector3 position, Vector3 normal, float force) {
		float newForce = base.Impact(position, normal, force);
		Debug.Log("Impact on Glass with " + force + "(" + newForce + ")" + " on " + normal);
		return newForce;
	}

}
