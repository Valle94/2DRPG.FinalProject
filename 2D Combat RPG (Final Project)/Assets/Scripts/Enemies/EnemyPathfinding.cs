using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] float moveSpeed = 4f;
    Vector2 moveDir; // Move Direction
    Rigidbody2D rb;

    // Initialize components in awake
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start() 
    {
        
    }

    
    void FixedUpdate()
    {
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
