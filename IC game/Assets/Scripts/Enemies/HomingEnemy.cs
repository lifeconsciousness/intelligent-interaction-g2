using UnityEngine;

public class HomingEnemy : Enemy
{
    public float homingStrength = 0.1f; // Adjust this value to control the steering strength

    protected override void Move()
    {
        if (!hasReachedPlane)
        {
            Vector3 planeNormal = player.forward;
            Vector3 planePoint = player.position;

            Plane movementPlane = new Plane(planeNormal, planePoint);
            float distanceToPlane = movementPlane.GetDistanceToPoint(transform.position);

            if (distanceToPlane > 0)
            {
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                moveDirection = Vector3.Lerp(moveDirection, directionToPlayer, homingStrength).normalized;
            }
            else
            {
                hasReachedPlane = true;
            }

            transform.position += moveDirection * speed * Time.deltaTime;

            if (moveDirection != Vector3.zero)
            {
                transform.forward = moveDirection;
            }
        }
        else
        {
            transform.position += moveDirection * speed * Time.deltaTime;

            if (moveDirection != Vector3.zero)
            {
                transform.forward = moveDirection;
            }
        }
    }
}