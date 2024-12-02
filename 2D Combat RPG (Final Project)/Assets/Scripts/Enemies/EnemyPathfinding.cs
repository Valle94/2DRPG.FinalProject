using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] float moveSpeed = 4f;
    Vector2 moveDir; // Move Direction
    Rigidbody2D rb;
    Knockback knockback;

    // Initialize components in awake
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        knockback = GetComponent<Knockback>();
    }

    void Start() 
    {
        
    }

    
    void FixedUpdate()
    {
        // If our enemy is 'getting knocked back' we will
        // return out of fixed update until the knockback 
        // is over, and then will go back to moving normally
        if (knockback.GettingKnockedBack)
        {
            return;
        }

        // Every update, move the enemy position based on current position,
        // movDir which is actually a target vector, and our moveSpeed.
        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));
    }

    // This method accepts a vector2 as an argument and sets
    // our moveDir to that vector.
    public void MoveTo(Vector2 targetPosition)
    {
        moveDir = targetPosition;
    }
}
