using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D playerRb; // Referencia al Rigidbody del jugador
    [SerializeField] PlayerInput playerInput; // Referencia al gestor de input
    [SerializeField] Animator playerAnim; // Referencia al Animator
    [SerializeField] private GameObject attackHitbox;
    private bool canAttack = true;

    [Header("Movement Parameters")]
    private Vector2 moveInput; // Almacena el input de movimiento
    public float speed;
    [SerializeField] bool isFacingRight;
    public bool playerWithRock;
    public NestInteraction homeNest;
    public NestInteraction currentNest;
    public bool canSlide = true;
    private float originalSpeed;

    private void Start()
    {
        originalSpeed = speed;
        playerRb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerAnim = GetComponent<Animator>();


        if (playerInput == null)
        {
            Debug.LogError("PlayerInput is missing from the GameObject.");
            return;
        }
        isFacingRight = true;
        playerInput.SwitchCurrentActionMap("Gameplay");
        playerInput.onActionTriggered += OnActionTriggered;
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
            //Debug.Log("funciona");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Slide();

        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            TryCollectRock();
        }

    }


    void Attack()
    {
        if (!canAttack || playerWithRock == true) return;

        playerAnim.SetTrigger("Hit");
        canAttack = false;
        StartCoroutine(AttackCooldown());
        attackHitbox.SetActive(true);
        StartCoroutine(DisableHitbox());
    }
    void Slide()
    {
        if (!canSlide) return;

        canSlide = false;


        float originalSpeed = speed;

        if (playerWithRock)
        {
            speed += 0.6f;
            StartCoroutine(ResetSpeedEarly(originalSpeed, 4f));
        }
        else
        {
            playerAnim.SetTrigger("Slide");
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


    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        canAttack = true;
    }
    IEnumerator DisableHitbox()
    {
        yield return new WaitForSeconds(0.2f);
        attackHitbox.SetActive(false);
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
        if (!playerWithRock)
        {
            playerWithRock = true;
            playerAnim.SetBool("HasRock", true);
            if (UiManager.Instance != null)
            {
                UiManager.Instance.UpdateUI();
            }
            else
            {
                Debug.LogError("UiManager.Instance es null.");
            }
        }
    }
    void TryCollectRock()
    {
        if (currentNest != null && currentNest.activeNest)
        {
            currentNest.HandleRockInteraction(this);
            if (UiManager.Instance != null)
            {
                UiManager.Instance.UpdateUI();
            }
            else
            {
                Debug.LogError("UiManager.Instance es null.");
            }
        }
    }

    public void DropRock()
    {
        if (playerWithRock) // Solo restaurar velocidad si tenía una roca antes
        {
            playerWithRock = false;
            playerAnim.SetBool("HasRock", false);
            speed = originalSpeed; // Restaurar velocidad original
            if (UiManager.Instance != null)
            {
                UiManager.Instance.UpdateUI();
            }
            else
            {
                Debug.LogError("UiManager.Instance es null.");
            }
        }
    }
}