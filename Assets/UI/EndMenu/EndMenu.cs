using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{

    [SerializeField]
    private Button _playAgainButton = default;

    void Awake()
    {
        _playAgainButton.onClick.AddListener(() => SceneManager.LoadScene("StartMenu"));
    }

}
