using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class SpawnEnemyEntity {
	public GameObject enemyPrefab;
	public Vector3 position;
	public bool noticePlayer = true;
	public string petriName;
}

public class SpawnEnemyTrigger : MonoBehaviour {

	[SerializeField]
	private List<SpawnEnemyEntity> m_EnemyList;

	void OnTriggerEnter(Collider other) {
		Player pl = other.GetComponent<Player>();
		if(pl) {
			foreach(SpawnEnemyEntity en in m_EnemyList) {
				GameObject ob = Instantiate(en.enemyPrefab, en.position, Quaternion.identity) as GameObject;
				EnemySpawner spawner = ob.GetComponent<EnemySpawner>();
				if(spawner) spawner.petriName = en.petriName;
			}
			Destroy(this.gameObject);
		}
	}

}

