using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

	private static AudioSource i_AudioSource;
	private static SoundController i_Instance;

	[SerializeField]
	[Range(0f, 1f)]
	private float m_MasterVolume = 1f;

	void Awake () {
		i_Instance = this;
		i_AudioSource = GetComponent<AudioSource>();
        ApplyVolume(PlayerPrefs.GetFloat("Volume", .2f));
	}

	public void ApplyVolume(float volume) {
		m_MasterVolume = volume;
	}

	public static void PlaySound (AudioClip audio, Vector3 position, float volume = 1f) {
		float soundVolume = volume * i_Instance.m_MasterVolume;
		if(soundVolume <= 0f) return;
		AudioSource.PlayClipAtPoint(audio, position, soundVolume);
	}
	
	public static void PlaySound (AudioSource source, AudioClip[] audiosClip) {
		if(audiosClip == null) return;
		if(audiosClip.Length == 0) return;

		AudioClip audio = audiosClip[Random.Range(0, audiosClip.Length)];
		PlaySound(source, audio);
	}
	
	public static void PlaySound (AudioSource source, AudioClip audioClip) {
		if(audioClip == null) return;
		
		float soundVolume = source.volume * i_Instance.m_MasterVolume;
		if(soundVolume <= 0f) return;
		source.PlayOneShot(audioClip, soundVolume);
	}
	
	public static void PlaySound (AudioSource source) {
		AudioClip audioClip = source.clip;
		float soundVolume = source.volume * i_Instance.m_MasterVolume;
		if(soundVolume <= 0f) return;
		source.PlayOneShot(audioClip, soundVolume);
	}
}
