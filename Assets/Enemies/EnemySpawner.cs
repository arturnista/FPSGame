using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemyPrefab;
	public string petriName;

	private AudioSource m_AudioSource;

	void Start () {
		m_AudioSource = GetComponent<AudioSource>();
		SoundController.PlaySound(m_AudioSource);
		if(enemyPrefab) {
			EnemyMovement enMov = Instantiate(enemyPrefab, transform.position + transform.up * 1f, Quaternion.identity).GetComponent<EnemyMovement>();;
			enMov.NoticePlayer();

			EnemyHealth health = enMov.GetComponent<EnemyHealth>();
			if(health) health.petriName = petriName;
		}
	}

}
