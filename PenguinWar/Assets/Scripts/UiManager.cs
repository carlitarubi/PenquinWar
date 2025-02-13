using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    public TextMeshProUGUI stoneCounterText;
    public Image rockIcon;
    public PlayerController player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateUI(); 
    }

    public void UpdateUI()
    {
        if (player == null)
        {
            Debug.LogError("UiManager: Player no está asignado.");
            return;
        }

        if (player.homeNest == null)
        {
            Debug.LogError("UiManager: homeNest del jugador es null.");
            return;
        }

        stoneCounterText.text = $"Your Rocks: {player.homeNest.activeRocks}";
        rockIcon.gameObject.SetActive(player.playerWithRock);
    }
}
