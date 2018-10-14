using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static GameController main;

	[Header("Physics")]
	public float gravity;

	void Awake() {
		main = this;
	}

	void Start () {
		Invoke ("StartGame", 1f);
	}

	void StartGame () {
		HUDController.main.ShowText ("Move the Payload outside");
	}

}