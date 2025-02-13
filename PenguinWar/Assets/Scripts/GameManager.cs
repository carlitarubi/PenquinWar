using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float timeLimit = 120f; 
    private float timer;
    public TextMeshProUGUI timerText; 
    public PlayerController player; 
    public NestInteraction playerNest; 
    public GameObject winPanel; 
    public GameObject losePanel; 

    void Start()
    {
        timer = timeLimit; 
        if (player == null) player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (playerNest == null) playerNest = player.homeNest;
    }

    void Update()
    {
        timer -= Time.deltaTime; 
        UpdateTimerUI();

        if (timer <= 0f)
        {
            CheckWinCondition();
        }
    }

    void UpdateTimerUI()
    {
        //timerText.text = "Tiempo restante: " + Mathf.Ceil(timer).ToString();
        timerText.text = Mathf.Ceil(timer).ToString();
    }

    void CheckWinCondition()
    {
        if (playerNest.activeRocks >= 5)
        {
            Win();
        }
        else
        {
            Lose();
        }
    }

    void Win()
    {
        winPanel.SetActive(true);
        Invoke("GoToNextScene", 3f); 
    }

    void Lose()
    {
        losePanel.SetActive(true);
        Invoke("RestartLevel", 3f);
    }

    void GoToNextScene()
    {
        SceneManager.LoadScene("NextScene");
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
