using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

	private static AudioSource i_AudioSource;

	void Awake () {
		i_AudioSource = GetComponent<AudioSource>();
	}
	
	public static void PlaySound (AudioClip audio, Vector3 position, float volume = 1f) {
		AudioSource.PlayClipAtPoint(audio, position, volume);
	}
}
