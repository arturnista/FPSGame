using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorActivate : Interactive {

	private Door m_Door;
	[SerializeField]
	private EnemyMovement[] m_EnemyToNotice; 

	void Awake () {
		m_Door = GetComponentInParent<Door>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void Use() {
		m_Door.Toggle();
		foreach(EnemyMovement en in m_EnemyToNotice) en.NoticePlayer();
	}
}
