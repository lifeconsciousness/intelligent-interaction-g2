using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float speed = 5f; // Movement speed of the enemy
    public float distanceThreshold = 0.1f; // Threshold to determine if the enemy has reached the plane
    public float timeAfterPlane = 2f; // Time to continue moving after reaching the plane

    protected bool hasReachedPlane = false; // Has the enemy reached the player's plane?
    protected float timer = 0f; // Timer after reaching the plane
    protected Vector3 moveDirection; // Current movement direction

    protected virtual void Start()
    {
        player = GameObject.Find("Player").transform;
        if (player == null)
        {
            Debug.LogWarning("Player not found!");
        }
    }

    protected virtual void Update()
    {
        Move();

        if (hasReachedPlane)
        {
            timer += Time.deltaTime;
            if (timer >= timeAfterPlane)
            {
                OnDestroyEnemy();
                Destroy(gameObject);
            }
        }
    }

    // Abstract method for movement; must be implemented by derived classes
    protected abstract void Move();

    protected virtual void OnReachedPlane()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
        transform.forward = moveDirection;
    }

    // Common destroy logic
    protected virtual void OnDestroyEnemy()
    {
        // Add common destruction effects or other things here
    }

    protected void CheckIfReachedPlane(Vector3 planeNormal, Vector3 planePoint)
    {
        Plane movementPlane = new Plane(planeNormal, planePoint);
        float distanceToPlane = movementPlane.GetDistanceToPoint(transform.position);

        if (!hasReachedPlane && Mathf.Abs(distanceToPlane) <= distanceThreshold)
        {
            hasReachedPlane = true;
            timer = 0f;
            OnReachedPlane();
        }
    }
}