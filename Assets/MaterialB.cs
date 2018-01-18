using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialB : MonoBehaviour {

    Material m_Material;

	void Awake () {
        m_Material = GetComponent<Renderer>().material;
		m_Material.mainTextureScale = new Vector2(this.transform.localScale.x, this.transform.localScale.z);
	}
	
}
