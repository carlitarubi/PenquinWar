using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyAI enemy = collision.GetComponent<EnemyAI>();

        if (enemy != null)
        {
            Debug.Log("Golpeó al enemigo, regresando al nido...");
            enemy.ForceReturnToNest();
        }
    }
}
