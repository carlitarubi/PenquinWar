using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D playerRb; // Referencia al rigidbody del player
    [SerializeField] PlayerInput playerInput; // Referencia al gestor del input del jugador
    [SerializeField] Animator playerAnim; // Referencia al animator para gestionar las transiciones de animación

    [Header("Movement Parameters")]
    private Vector2 moveInput; //Almacén del input del player
    public float speed;
    [SerializeField] bool isFacingRight;
    public bool playerWithRock;
    public NestInteraction homeNest;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerAnim = GetComponent<Animator>();
        isFacingRight = true;
    }

    void Update()
    {
        playerRb.velocity = new Vector2(moveInput.x * speed, moveInput.y * speed);


        if (moveInput.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput.x < 0 && isFacingRight)
        {
            Flip();
        }
        // Comprobación de la entrada para colocar una piedra
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            TryPlaceRock();
        }
    }

    private void TryPlaceRock()
    {
        // Verificar si el jugador tiene una piedra y si su nido no está lleno
        if (playerWithRock && homeNest.activeRocks < homeNest.maxRocks)
        {
            homeNest.rockPrefabs[homeNest.activeRocks].SetActive(true); // Activar una piedra en su nido
            homeNest.activeRocks++; // Incrementar las piedras activas en el nido del jugador
            playerWithRock = false; // El jugador ya no tiene la piedra
        }
        // Verificar si el jugador no tiene piedra para quitar una roca de su nido
        else if (!playerWithRock && homeNest.activeRocks > 0)
        {
            homeNest.activeRocks--; // Reducir el número de piedras activas en su nido
            homeNest.rockPrefabs[homeNest.activeRocks].SetActive(false); // Desactivar la última piedra en su nido
            playerWithRock = true; // El jugador ahora tiene la piedra
        }
    }
    

    

    void Flip()
    {
        Vector2 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
        isFacingRight = !isFacingRight;
    }


    public void HandleMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void CollectRock()
    {
        playerWithRock = true; // El jugador recoge una piedra
    }

    // Lógica para soltar la piedra (esto puede estar en otro sistema si lo deseas)
    public void DropRock()
    {
        playerWithRock = false; // El jugador suelta la piedra
    }
}