using UnityEngine;

public class HomingEnemy : Enemy
{
    public float homingStrength = 0.5f; // How strongly the projectile adjusts toward the player

    protected override void Move()
    {
        if (!hasReachedPlane)
        {
            // Calculate direction toward the player
            Vector3 targetDirection = (player.position - transform.position).normalized;

            // Interpolate between the current move direction and the target direction
            moveDirection = Vector3.Lerp(moveDirection, targetDirection, homingStrength * Time.deltaTime).normalized;

            // Check if the enemy is close enough to the player (to simulate reaching the "plane")
            if (Vector3.Distance(transform.position, player.position) <= distanceThreshold)
            {
                hasReachedPlane = true;
            }

            // Move the enemy
            transform.position += moveDirection * speed * Time.deltaTime;
            transform.forward = moveDirection;
        }
        else
        {
            // Continue moving in the same direction after reaching the "plane"
            transform.position += moveDirection * speed * Time.deltaTime;
            transform.forward = moveDirection;
        }
    }
}
