using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialType : MonoBehaviour {

	protected MaterialTypeConfig m_Config;
	protected Renderer m_Renderer;

	protected virtual void Awake () {
		m_Renderer = this.GetComponent<Renderer>();
	}

	protected void Config(string name) {
		m_Config = MaterialTypesController.i_Instance.GetConfig(name);
	}
	
	public virtual float Impact(Vector3 position, Vector3 normal, float force) {
		float newForce = force * m_Config.penetration;

		this.CreateDecal(position, normal, newForce > 0f);

		return newForce;
	}

	protected virtual void CreateDecal(Vector3 position, Vector3 normal, bool doubleSide) {
		GameObject impactPrefab = m_Config.bulletHoleDecals[Random.Range(0, m_Config.bulletHoleDecals.Length)];

		GameObject imp = Instantiate(impactPrefab, position, Quaternion.LookRotation(normal * -1)) as GameObject;
		imp.transform.position -= imp.transform.forward * .01f;
		imp.transform.SetParent(transform);

		if(doubleSide) {
			GameObject imp2 = Instantiate(impactPrefab, position, Quaternion.LookRotation(normal)) as GameObject;

			Vector3 impDirection = normal;
			impDirection.Scale(m_Renderer.bounds.size);
			float mult = impDirection.magnitude + .01f;

			imp2.transform.position -= imp2.transform.forward * mult;
			imp2.transform.SetParent(transform);
		}
	}
}
