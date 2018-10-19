using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

	private ButtonText m_NewGameButton;
	private ButtonText m_OptionsButton;

	private Canvas m_StartMenuCanvas;

	#region Options
	private Canvas m_OptionsCanvas;

	private ButtonText m_BackButton;
	private ButtonText m_SaveButton;

	private Slider m_SensibilitySlider;
	private TextMeshProUGUI m_SensibilityValue;

	private Slider m_VolumeSlider;
	private TextMeshProUGUI m_VolumeValue;
	#endregion

	void Awake () {
		m_StartMenuCanvas = transform.Find("StartMenuCanvas").GetComponent<Canvas>();
		m_OptionsCanvas = transform.Find("OptionsCanvas").GetComponent<Canvas>();

		OpenStartMenu();

		m_NewGameButton = transform.Find("StartMenuCanvas/NewGameButton").GetComponent<ButtonText>();
		
		m_NewGameButton.onClick.AddListener(() => {
			SceneManager.LoadScene("Game", LoadSceneMode.Single);
		});

		// Options Menu
		m_OptionsButton = transform.Find("StartMenuCanvas/OptionsButton").GetComponent<ButtonText>();
		
		m_OptionsButton.onClick.AddListener(() => {
			DefaultOptions();
			OpenOptionsMenu();
		});

		m_BackButton = transform.Find("OptionsCanvas/BackButton").GetComponent<ButtonText>();
		m_SaveButton = transform.Find("OptionsCanvas/SaveButton").GetComponent<ButtonText>();

		m_BackButton.onClick.AddListener(() => {
			OpenStartMenu();
		});

		m_SaveButton.onClick.AddListener(() => {
			SaveOptions();
			OpenStartMenu();
		});

		m_SensibilitySlider = transform.Find("OptionsCanvas/SensibilityContainer/Slider").GetComponent<Slider>();
		m_SensibilityValue = transform.Find("OptionsCanvas/SensibilityContainer/Value").GetComponent<TextMeshProUGUI>();

		m_SensibilitySlider.onValueChanged.AddListener((float value) => {
			m_SensibilityValue.text = string.Format("{0,12:#.00}", value);
		});

		m_VolumeSlider = transform.Find("OptionsCanvas/VolumeContainer/Slider").GetComponent<Slider>();
		m_VolumeValue = transform.Find("OptionsCanvas/VolumeContainer/Value").GetComponent<TextMeshProUGUI>();

		m_VolumeSlider.onValueChanged.AddListener((float value) => {
			m_VolumeValue.text = string.Format("{0} %", value);
		});
	}

	void OpenOptionsMenu() {
		m_StartMenuCanvas.gameObject.SetActive(false);
		m_OptionsCanvas.gameObject.SetActive(true);
	}

	void OpenStartMenu() {
		m_StartMenuCanvas.gameObject.SetActive(true);
		m_OptionsCanvas.gameObject.SetActive(false);
	}

	void DefaultOptions() {
		m_SensibilitySlider.value = GameOptions.main.sensibility;
		m_VolumeSlider.value = GameOptions.main.soundVolume;

		m_SensibilityValue.text = string.Format("{0,12:#.00}", m_SensibilitySlider.value);
		m_VolumeValue.text = string.Format("{0} %", m_VolumeSlider.value);
	}

	void SaveOptions() {
		GameOptions.main.SetOptions(m_SensibilitySlider.value, m_VolumeSlider.value);
		Debug.Log("Options saved!");
	}
	
	// Update is called once per frame
	void Update () {

	}
}
