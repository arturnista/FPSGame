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
		Invoke ("StartGame", 2f);
	}

	void StartGame () {
		HUDController.main.ShowText ("Thanks for testing the game!");
	}

}