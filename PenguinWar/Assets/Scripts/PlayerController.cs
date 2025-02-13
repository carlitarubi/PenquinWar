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
    public bool canSlide = true;


    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerAnim = GetComponent<Animator>();
        isFacingRight = true;

        playerInput.SwitchCurrentActionMap("Gameplay");

        playerInput.onActionTriggered += OnActionTriggered;
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput is missing from the GameObject.");
            return; 
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
        if (!canSlide) return;

        canSlide = false;
        playerAnim.SetTrigger("Slide");

        float originalSpeed = speed;

        if (playerWithRock)
        {
            speed += 0.6f;
            StartCoroutine(ResetSpeedEarly(originalSpeed, 4f));
        }
        else
        {
            speed += 6f; 
            StartCoroutine(ResetSpeed(originalSpeed, 0.5f)); 
        }
        StartCoroutine(SlideCooldown(1.5f));
    }
    IEnumerator ResetSpeed(float originalSpeed, float delay)
    {
        yield return new WaitForSeconds(delay);
        speed = originalSpeed;
    }
    IEnumerator ResetSpeedEarly(float originalSpeed, float delay)
    {
        float elapsedTime = 0f;

        while (elapsedTime < delay)
        {
            if (!playerWithRock)
            {
                speed = originalSpeed;
                yield break;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        speed = originalSpeed;
    }

    IEnumerator SlideCooldown(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        canSlide = true;
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
        speed *= 0.4f; 
    }


    public void DropRock()
    {
        playerWithRock = false;
        speed /= 0.4f; 
    }
}