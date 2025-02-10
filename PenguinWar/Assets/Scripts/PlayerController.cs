using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D playerRb; // Referencia al rigidbody del player
    [SerializeField] PlayerInput playerInput; // Referencia al gestor del input del jugador
    [SerializeField] Animator playerAnim; // Referencia al animator para gestionar las transiciones de animaci�n

    [Header("Movement Parameters")]
    private Vector2 moveInput; //Almac�n del input del player
    public float speed;
    [SerializeField] bool isFacingRight;
    public bool playerWithRock;


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
    }

    //private void FixedUpdate()
    //{

        
    //}

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

    // L�gica para soltar la piedra (esto puede estar en otro sistema si lo deseas)
    public void DropRock()
    {
        playerWithRock = false; // El jugador suelta la piedra
    }
}