using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public TextMeshProUGUI stoneCounterText; 
    public Image rockIcon; 
    public PlayerController player; 
    private void Update()
    {
        UpdateUI();
    }
    void UpdateUI()
    {
        stoneCounterText.text = "Piedras en nido: " + player.homeNest.activeRocks;
        rockIcon.gameObject.SetActive(player.playerWithRock);
    }
}
