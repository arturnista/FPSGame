using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHoleDecal : MonoBehaviour {
	
	[SerializeField]
	private float m_Duration = 1f;
	private float m_PassedTime;

	void Start() {
		float angle = Random.Range(0f, 360f);
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle);
	}
	
	// Update is called once per frame
	void Update () {
		m_PassedTime += Time.deltaTime;
		if(m_PassedTime >= m_Duration) {
			Destroy(this.gameObject);
		}
	}
}
