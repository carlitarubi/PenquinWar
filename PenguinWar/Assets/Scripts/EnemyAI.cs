using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public NestInteraction[] nests;
    public NestInteraction homeNest;
    public float moveSpeed = 2f;
    public float waitTimeAfterStealing = 2f;

    private NestInteraction targetNest;
    private float stateTimer;
    private bool hasRock = false; // Indica si el enemigo lleva una piedra
    private EnemyState currentState = EnemyState.Idle;

    private enum EnemyState
    {
        Idle,
        MovingToNest,
        StealingRock,
        ReturningHome,
        Waiting
    }

    void Start()
    {
        SetState(EnemyState.Idle);
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                ChooseRandomNest();
                break;

            case EnemyState.MovingToNest:
                MoveToTarget(targetNest.transform.position, EnemyState.StealingRock);
                break;

            case EnemyState.StealingRock:
                StealRock();
                break;

            case EnemyState.ReturningHome:
                MoveToTarget(homeNest.transform.position, EnemyState.Waiting);
                break;

            case EnemyState.Waiting:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0f)
                {
                    SetState(EnemyState.Idle);
                }
                break;
        }
    }

    private void SetState(EnemyState newState)
    {
        currentState = newState;
        if (newState == EnemyState.Waiting)
        {
            stateTimer = waitTimeAfterStealing;
            if (hasRock)
            {
                AddRockToHomeNest(); // Añadir piedra al nido propio si lleva una
                hasRock = false; // Vaciar la piedra del enemigo
            }
        }
    }

    private void ChooseRandomNest()
    {
        if (nests.Length == 0) return;

        do
        {
            targetNest = nests[Random.Range(0, nests.Length)];
        } while (targetNest == homeNest);

        SetState(EnemyState.MovingToNest);
    }

    private void MoveToTarget(Vector3 destination, EnemyState nextState)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, destination) < 0.1f)
        {
            SetState(nextState);
        }
    }

    private void StealRock()
    {
        if (targetNest.activeRocks > 0)
        {
            targetNest.EnemyDestroysRock();
            Debug.Log($"{gameObject.name} robó una piedra del nido {targetNest.name}");
            hasRock = true; // Indicar que el enemigo lleva una piedra
        }

        SetState(EnemyState.ReturningHome);
    }

    private void AddRockToHomeNest()
    {
        if (homeNest.activeRocks < homeNest.maxRocks)
        {
            homeNest.rockPrefabs[homeNest.activeRocks].SetActive(true);
            homeNest.activeRocks++;
            Debug.Log($"{gameObject.name} añadió una piedra a su nido {homeNest.name}");
        }
    }
}
