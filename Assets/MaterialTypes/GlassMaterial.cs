using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassMaterial : MaterialType {

	
	public override void Impact(Vector3 position, Vector3 normal) {
		base.Impact(position, normal);
		Debug.Log("Impact on Glass with " + normal);
	}

}
