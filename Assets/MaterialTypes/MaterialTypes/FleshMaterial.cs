using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BodyPart))]
public class FleshMaterial : MaterialType {

	private BodyPart m_BodyPart;

	void Start () {
		this.Config("flesh");
		m_BodyPart = GetComponent<BodyPart>();
	}

	public override float Impact(Vector3 position, Vector3 normal, float force) {
		float newForce = base.Impact(position, normal, force);
		// Debug.Log("Impact on Flesh with " + force + "(" + newForce + ")" + " on " + normal);

		m_BodyPart.DealDamage(force);

		return newForce;
	}

}
