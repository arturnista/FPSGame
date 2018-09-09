using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayOnAwake : MonoBehaviour {

	private AudioSource m_AudioSource;

	void Start () {
		m_AudioSource = GetComponent<AudioSource> ();
		SoundController.PlaySound (m_AudioSource);
	}

}