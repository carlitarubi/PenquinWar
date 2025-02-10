using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public bool hasRock = false; // Indica si el enemigo tiene una piedra
    private Vector3 targetPosition; // La posición de destino a la que el enemigo se mueve
    private NestInteraction currentNest; // Referencia al nido al que el enemigo se dirige

    private bool isMovingBackToNest = false; // Indicador de si el enemigo está regresando al nido
    private bool isWaiting = false; // Para evitar que el enemigo se mueva sin parar

    private void Start()
    {
        // Asignar el primer nido de manera aleatoria
        currentNest = FindObjectsOfType<NestInteraction>()[Random.Range(0, 5)];
        targetPosition = currentNest.transform.position;
    }

    private void Update()
    {
        // Verificar si el enemigo tiene una piedra o está en su nido
        if (hasRock)
        {
            // Si tiene una piedra, debe regresar a su nido
            if (Vector3.Distance(transform.position, currentNest.transform.position) < 0.5f)
            {
                ReturnRock(); // Devolver la piedra al nido
            }
            else
            {
                // Moverse hacia el nido de manera directa si tiene una piedra
                MoveToTarget();
            }
        }
        else
        {
            // Si no tiene una piedra, moverse aleatoriamente entre los nidos
            if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
            {
                // Si llega al nido, intentar robar una piedra
                TryToStealRock();
                StartCoroutine(RandomMovement()); // Empezar a moverse a otro nido
            }
            else
            {
                MoveToTarget(); // Continuar moviéndose hacia el objetivo (nido aleatorio)
            }
        }
    }

    // Mover al enemigo hacia su objetivo
    void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    // Intentar robar una piedra si está en un nido
    void TryToStealRock()
    {
        if (currentNest.activeRocks > 0)
        {
            hasRock = true; // El enemigo ha robado una piedra
            currentNest.activeRocks--; // Reducir las piedras activas del nido
            Debug.Log("Enemigo robó una piedra");
        }
    }

    // Función que devuelve la piedra al nido
    void ReturnRock()
    {
        currentNest.activeRocks++; // Aumenta las piedras del nido
        hasRock = false; // El enemigo ya no tiene la piedra
        Debug.Log("Enemigo ha devuelto la piedra al nido");
        StartCoroutine(RandomMovement()); // Después de devolver la piedra, empezar a moverse aleatoriamente
    }

    // Mover al enemigo aleatoriamente entre los nidos
    IEnumerator RandomMovement()
    {
        // Esperamos un poco para simular un tiempo de espera
        yield return new WaitForSeconds(1f);

        // Elegimos un nido aleatorio de los disponibles
        NestInteraction[] nests = FindObjectsOfType<NestInteraction>();
        currentNest = nests[Random.Range(0, nests.Length)];

        // Establecemos el nuevo destino
        targetPosition = currentNest.transform.position;

        // Esperamos antes de que el enemigo comience a moverse
        isWaiting = false;
    }
}
