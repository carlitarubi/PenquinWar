using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D playerRb; // Referencia al Rigidbody del jugador
    [SerializeField] PlayerInput playerInput; // Referencia al gestor de input
    [SerializeField] Animator playerAnim; // Referencia al Animator

    [Header("Movement Parameters")]
    private Vector2 moveInput; // Almacena el input de movimiento
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

        // Asegurar que el input está en el modo correcto
        playerInput.SwitchCurrentActionMap("Gameplay");

        // Vincular el input
        playerInput.onActionTriggered += OnActionTriggered;
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput is missing from the GameObject.");
            return; // Si falta el PlayerInput, no sigas ejecutando el resto del código
        }
    }

    void Update()
    {
        playerRb.velocity = new Vector2(moveInput.x * speed, moveInput.y * speed);
        playerAnim.SetFloat("Speed", Mathf.Abs(moveInput.magnitude));

        if (moveInput.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput.x < 0 && isFacingRight)
        {
            Flip();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {

            Attack();
            Debug.Log("funciona");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Slide();
            
        }

    }
  

    void Attack()
    {
        playerAnim.SetTrigger("Hit");



        EnemyAI enemy = GetComponent<EnemyAI>();

        if (enemy != null && enemy.hasRock)
        {
            Vector3 enemyPosition = enemy.transform.position;
            Debug.Log("Enemy hit at position: " + enemyPosition);
            enemy.hasRock = false;
            
        }
        else
        {
            Debug.Log("No EnemyAI component found or enemy doesn't have a rock.");
        }
    }
    void Slide()
    {
        if (playerWithRock)
        {
            //playerAnim.SetTrigger("Dodge");
        }
        else
        {
            playerAnim.SetTrigger("Slide");
            speed = 7;
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

    private void OnActionTriggered(InputAction.CallbackContext context)
    {
        if (context.action.name == "Move")
        {
            HandleMove(context);
        }
    }

    public void CollectRock()
    {
        playerWithRock = true;
    }

    public void DropRock()
    {
        playerWithRock = false;
    }
}