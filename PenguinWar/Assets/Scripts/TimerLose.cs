using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerLose : MonoBehaviour
{
    public float timeLimit = 5f;
    private float timer;
    public TextMeshProUGUI timerText;
    void Start()
    {
        timer = timeLimit;
    }
    void Update()
    {
        timer -= Time.deltaTime;
        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
       
        timerText.text = Mathf.Ceil(timer).ToString();
    }
    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}


   