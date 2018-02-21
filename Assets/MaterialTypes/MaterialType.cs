using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialType : MonoBehaviour {

	public GameObject impactPrefab;

	protected virtual void Awake () {
		
	}
	
	public virtual void Impact(Vector3 position, Vector3 normal) {
		GameObject imp = Instantiate(impactPrefab, position, Quaternion.LookRotation(normal * -1)) as GameObject;
		imp.transform.position -= imp.transform.forward * .01f;

		imp.transform.SetParent(transform);
	}
}
