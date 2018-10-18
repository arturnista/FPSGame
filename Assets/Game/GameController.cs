using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static GameController main;

	[Header("Constants")]
	[SerializeField]
	private bool m_PowerIsOn = true;
	public bool powerIsOn {
		get {
			return m_PowerIsOn;
		}
	}

	[Header("Physics")]
	public float gravity;

	void Awake() {
		main = this;
		m_PowerIsOn = true;
	}

	void Start () {
		Invoke ("StartGame", 1f);
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.L)) {
			DisablePower();
		} else if(Input.GetKeyDown(KeyCode.O)) {
			EnablePower();
		}
	}

	void StartGame () {
		HUDController.main.ShowText ("Move the Payload outside");
	}

	void DisablePower() {
		m_PowerIsOn = false;

		Lamp[] lamps = GameObject.FindObjectsOfType<Lamp>();
		foreach(Lamp l in lamps) l.Off();

		MovingObject[] mObjects = GameObject.FindObjectsOfType<MovingObject>();
		foreach(MovingObject l in mObjects) l.Off();
	}

	void EnablePower() {
		m_PowerIsOn = true;

		Lamp[] Lamps = GameObject.FindObjectsOfType<Lamp>();
		foreach(Lamp l in Lamps) l.On();

		MovingObject[] mObjects = GameObject.FindObjectsOfType<MovingObject>();
		foreach(MovingObject l in mObjects) l.On();
	}

}