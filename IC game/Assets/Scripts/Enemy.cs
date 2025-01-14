using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float speed = 5f; // Movement speed of the enemy
    public float distanceThreshold = 0.1f; // Threshold to determine if the enemy has reached the plane
    public float timeAfterPlane = 2f; // Time to continue moving after reaching the plane
    public float damageCooldownTime = 2f; // Cooldown between damage ticks

    protected bool hasReachedPlane = false; // Has the enemy reached the player's plane?
    protected float timer = 0f; // Timer after reaching the plane
    protected Vector3 moveDirection; // Current movement direction

    protected Collider playerCollider;
    protected Collider enemyCollider;
    private GameManager gameManager;
    
    private float damageCooldown = 0f; // Cooldown between damage ticks

    public int damage = 10; // Damage dealt to the player on collision

    public Vector3 originalPosition;
    

    protected virtual void Start()
    {
        originalPosition = transform.position;
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogWarning("Game Manager not found!");
        }

        player = GameObject.Find("Player").transform;
        if (player == null)
        {
            Debug.LogWarning("Player not found!");
        }

        playerCollider = player.GetComponent<Collider>();
        if (playerCollider == null)
        {
            Debug.LogWarning("Player collider not found!");
        }

        enemyCollider = GetComponent<Collider>();
        if (enemyCollider == null)
        {
            Debug.LogWarning("Enemy collider not found!");
        }
    }

    protected virtual void Update()
    {
        Move();

        if (playerCollider.bounds.Intersects(enemyCollider.bounds))
        {
            onPlayerCollision();
        }

        if (damageCooldown > 0f)
        {
            damageCooldown -= Time.deltaTime;
        }

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
        // Add any specific behavior when the enemy reaches the plane if needed
    }

    // Common destroy logic
    protected virtual void OnDestroyEnemy()
    {
        // Add common destruction effects or other things here
    }

    protected virtual void onPlayerCollision()
    {
        // Deal damage to the player on collision but only once per second
        if (damageCooldown <= 0f)
        {
            gameManager.TakeDamage(damage);
            damageCooldown = damageCooldownTime;
        }
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