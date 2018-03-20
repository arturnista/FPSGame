using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour {

	[SerializeField]
	private EnemyMovement[] m_EnemyToNotice; 

	public virtual void Use() {
		foreach(EnemyMovement en in m_EnemyToNotice) {
			if(en) en.NoticePlayer();
		}
	}
}
