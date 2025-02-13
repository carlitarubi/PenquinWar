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
    private bool gameEnded = false;

    private bool isPaused = false;

    void Start()
    {
        timer = timeLimit; 
        if (player == null) player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (playerNest == null) playerNest = player.homeNest;
    }

    void Update()
    {
        if (!gameEnded)
        {
            timer -= Time.deltaTime;
            UpdateTimerUI();

            if (timer <= 0f)
            {
                CheckWinCondition();
            }
        }
    }

    void UpdateTimerUI()
    {
        //timerText.text = "Tiempo restante: " + Mathf.Ceil(timer).ToString();
        timerText.text = Mathf.Ceil(timer).ToString();
    }

    void CheckWinCondition()
    {
        if (playerNest.activeRocks >= 1)
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
        gameEnded = true;
        PauseGame();
        winPanel.SetActive(true);
        Invoke("GoToNextScene", 2f);
    }

    void Lose()
    {
        gameEnded = true;
        PauseGame();
        losePanel.SetActive(true);
        Invoke("RestartLevel", 2f);
    }

    void GoToNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }
    void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }
}
