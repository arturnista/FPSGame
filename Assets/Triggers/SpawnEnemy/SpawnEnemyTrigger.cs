using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class SpawnEnemyEntity {
	public GameObject enemyPrefab;
	public Vector3 position;
	public bool noticePlayer = true;
}

public class SpawnEnemyTrigger : MonoBehaviour {

	[SerializeField]
	private List<SpawnEnemyEntity> m_EnemyList;

	void OnTriggerEnter(Collider other) {
		Player pl = other.GetComponent<Player>();
		if(pl) {
			foreach(SpawnEnemyEntity en in m_EnemyList) {
				EnemyMovement enMov = Instantiate(en.enemyPrefab, en.position, Quaternion.identity).GetComponent<EnemyMovement>();
				if(en.noticePlayer) enMov.NoticePlayer();
			}
			Destroy(this.gameObject);
		}
	}
}
