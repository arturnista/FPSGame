using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadStopTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider coll) {
		PayloadBehaviour payload = coll.transform.GetComponent<PayloadBehaviour>();
		if(payload) {
			payload.Stop();
		}
	}
}
