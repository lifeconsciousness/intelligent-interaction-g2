using UnityEngine;

public class HomingEnemy : Enemy
{
    public float homingStrength = 0.5f; // How strongly the projectile adjusts toward the player

    protected override void Move()
    {
        // Always calculate the direction toward the player
        Vector3 targetDirection = (player.position - transform.position).normalized;

        if (!hasReachedPlane)
        {
            // Interpolate between the current move direction and the target direction
            moveDirection = Vector3.Lerp(moveDirection, targetDirection, homingStrength * Time.deltaTime).normalized;

            // Move the enemy
            transform.position += moveDirection * speed * Time.deltaTime;
            transform.forward = moveDirection;

            // Optionally remove the hasReachedPlane check if it's no longer needed
        }
        else
        {
            // Continue moving in the same direction after reaching the \"plane\"
            transform.position += moveDirection * speed * Time.deltaTime;
            transform.forward = moveDirection;
        }
    }

    protected override void onPlayerCollision()
    {
        // Ensure collision only happens when the colliders physically intersect
        if (playerCollider.bounds.Intersects(enemyCollider.bounds))
        {
            base.onPlayerCollision();
        }
    }
}
