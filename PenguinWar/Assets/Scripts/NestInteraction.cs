using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestInteraction : MonoBehaviour
{
    public GameObject[] rockPrefabs; // Arreglo de las 8 piedras (4 visibles, 4 invisibles)
    public int maxRocks = 8; // Número máximo de piedras (8)
    public int activeRocks = 4; // Al principio hay 4 piedras visibles
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
        // Si el jugador está cerca y presiona la tecla Mayus (Shift)
        if (playerNear && Input.GetKeyDown(KeyCode.LeftShift))
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                // Verificar si el jugador tiene la piedra (playerWithRock es true)
                var playerScript = player.GetComponent<PlayerController>();
                if (playerScript != null && playerScript.playerWithRock)
                {
                    // Si hay piedras por colocar
                    if (activeRocks < maxRocks)
                    {
                        rockPrefabs[activeRocks].SetActive(true); // Activar una piedra invisible
                        activeRocks++;
                    }
                }
            }
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
