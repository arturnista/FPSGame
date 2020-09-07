using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameController : MonoBehaviour
{
    
    public static EndGameController Instance { get; private set; }
	
    public GameObject boss;
	private HUDController m_HUDController;

    void Awake()
    {
        Instance = this;
		m_HUDController = GameObject.FindObjectOfType<HUDController>();
    }

    public void CreateBoss()
    {
		boss.SetActive(true);
        boss.GetComponent<BossHealth>().OnDeath += HandleBossDeath;
    }

    void HandleBossDeath()
    {
        StartCoroutine(EndGameCoroutine());
    }

    IEnumerator EndGameCoroutine()
    {
        yield return new WaitForSeconds(5f);
        m_HUDController.EndGame();
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("EndMenu");
    }

}
