using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

	private ButtonText mNewGameButton;

	void Awake () {
		mNewGameButton = GameObject.Find("NewGameButton").GetComponent<ButtonText>();
		
		mNewGameButton.onClick.AddListener(() => {
			SceneManager.LoadScene("Game", LoadSceneMode.Single);
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
