using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOptions : MonoBehaviour {

	public static GameOptions main;
    
    private float m_Sensibility = 3f;
    private float m_SoundVolume = .2f;

    public float sensibility {
        get {
            return m_Sensibility;
        }
    }

    public float soundVolume {
        get {
            return Mathf.Round(m_SoundVolume * 100f);
        }
    }

	void Awake() {
        if(main) {
            Destroy(gameObject);
            return;
        }
		main = this;
        DontDestroyOnLoad(gameObject);
	}

    public void SetOptions(float sensibility, float volume) {
        m_Sensibility = sensibility;
        m_SoundVolume = volume / 100f;
    }

    public void ApplyOptions() {
        SoundController sc = GameObject.FindObjectOfType<SoundController>();
        sc.ApplyVolume(m_SoundVolume);

        PlayerLook pl = GameObject.FindObjectOfType<PlayerLook>();
        pl.ApplySensibility(m_Sensibility);
    }

}