using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestInteraction : MonoBehaviour
{
    public GameObject[] rockPrefabs; // Array de piedras en el nido
    public int maxRocks = 8;
    public int activeRocks = 4;
    private bool playerNearEnemyNest = false; 
    private bool playerNearPlayerNest = false; 
    public bool isPlayerNest = false;
    public bool activeNest = false;

    private PlayerController player;

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.GetComponent<PlayerController>();
        if (player == null) Debug.LogError("Player not found!");

        
        for (int i = 0; i < rockPrefabs.Length; i++)
        {
            rockPrefabs[i].SetActive(i < activeRocks);
        }
    }

    void Update()
    {
        
    }

    private void TakeRock(PlayerController player)
    {
        
        if (activeRocks > 0 && !player.playerWithRock) 
        {
            activeRocks--;
            rockPrefabs[activeRocks].SetActive(false);
            player.playerWithRock = true;
          
        }
       
    }

    private void PlaceRock(PlayerController player)
    {
        if (activeRocks < maxRocks && player.playerWithRock && playerNearPlayerNest) 
        {
            rockPrefabs[activeRocks].SetActive(true);
            activeRocks++;
            player.playerWithRock = false;
           
        }
       
    }

    
    

    // Función para que los enemigos destruyan piedras
    public void EnemyDestroysRock()
    {
        if (activeRocks > 0)
        {
            activeRocks--;
            rockPrefabs[activeRocks].SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            activeNest = true;

            if (Input.GetKeyDown(KeyCode.LeftShift) && activeNest)
            {
                if (!isPlayerNest && !player.playerWithRock)
                {
                    TakeRock(player);
                }
                if (isPlayerNest && player.playerWithRock)
                {
                    PlaceRock(player);
                }

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            activeNest = false;
        }
    }
}