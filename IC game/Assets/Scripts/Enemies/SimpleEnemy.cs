using UnityEngine;

public class SimpleEnemy : Enemy
{
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
                Vector3 projectedPosition = transform.position - planeNormal * distanceToPlane;

                moveDirection = (projectedPosition - transform.position).normalized;
            }


            transform.position += moveDirection * speed * Time.deltaTime;
            transform.forward = moveDirection;

            CheckIfReachedPlane(planeNormal, planePoint);
        }
        else
        {
            transform.position += moveDirection * speed * Time.deltaTime;
            transform.forward = moveDirection;
        }
    }
}