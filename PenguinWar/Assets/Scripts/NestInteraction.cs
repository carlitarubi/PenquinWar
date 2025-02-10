using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestInteraction : MonoBehaviour
{
    public GameObject[] rockPrefabs; // Array de piedras
    public int maxRocks = 8;
    public int activeRocks = 4; 
    private bool playerNear = false; // Para saber si el jugador está cerca

    void Start()
    {
        // Iniciar las piedras, las primeras 4 son visibles y las siguientes 4 son invisibles
        for (int i = 0; i < rockPrefabs.Length; i++)
        {
            if (i >= activeRocks) // Las piedras que no deben ser visibles al inicio
                rockPrefabs[i].SetActive(false);
        }
    }

    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.LeftShift))
        {
            TryPlaceRock();
        }
    }

    private void TryPlaceRock()
    {
        var player = GameObject.FindWithTag("Player")?.GetComponent<PlayerController>();

        // Verificar si el jugador tiene una piedra y si el nido no está lleno
        if (player != null && player.playerWithRock && activeRocks < maxRocks)
        {
            rockPrefabs[activeRocks].SetActive(true); // Activar una piedra invisible
            activeRocks++;
            player.playerWithRock = false; // El jugador ya no tiene la piedra después de ponerla
        }
        // Verificar si el jugador no tiene piedra para quitar una roca
        else if (player != null && !player.playerWithRock && activeRocks > 0)
        {
            activeRocks--; // Reducir el número de piedras activas
            rockPrefabs[activeRocks].SetActive(false); // Desactivar la última piedra
            player.playerWithRock = true; // El jugador ahora tiene la piedra
        }
    }


    // Detectar si el jugador está cerca del nido
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true; // El jugador está cerca
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false; // El jugador ya no está cerca
        }
    }

    // Función para que los enemigos destruyan piedras
    public void EnemyDestroysRock()
    {
        if (activeRocks > 0)
        {
            activeRocks--; // Restar una piedra activa
            rockPrefabs[activeRocks].SetActive(false); // Desactivar la piedra
        }
    }
}
