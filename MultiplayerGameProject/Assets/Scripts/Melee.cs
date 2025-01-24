using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public float attackRange = 2f; // Range of the melee attack
    public float attackAngle = 60f; // Angle of the attack cone
    public LayerMask playerLayer; // Layer mask to identify players
    public float attackCooldown = 0.5f; // Cooldown time between attacks

    private float lastAttackTime = 0f; // Tracks the time of the last attack

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown) // Check for left mouse button press and cooldown
        {
            PerformMeleeAttack();
            lastAttackTime = Time.time; // Update the time of the last attack
        }
    }

    void PerformMeleeAttack()
    {
        // Get all colliders within the attack range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, playerLayer);

        foreach (Collider collider in hitColliders)
        {
            // Skip if the collider belongs to the same GameObject or its children
            if (collider.gameObject == gameObject) continue;

            // Calculate direction to the target
            Vector3 directionToTarget = collider.transform.position - transform.position;
            directionToTarget.y = 0; // Ignore vertical difference

            // Check if the target is within the attack cone
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
            if (angleToTarget <= attackAngle / 2)
            {
                Debug.Log($"Player hit: {collider.name}");
                //To be used for taking damage
                //if(collider.gameObject.GetComponent<PlayerMovementJ>() != null)
                    //collider.GetComponent<PlayerMovementJ>().TakeDamage(); 
            }
        }
    }

    // To visualize the attack area in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Draw attack cone (simplified visualization)
        Vector3 forward = transform.forward * attackRange;
        Vector3 leftBoundary = Quaternion.Euler(0, -attackAngle / 2, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, attackAngle / 2, 0) * forward;

        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }
}
