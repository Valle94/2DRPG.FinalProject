using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    // This script is attached to our weapon collider 'hitbox'
    void OnTriggerEnter2D(Collider2D other) 
    {
        // If our hitbox collides with an object that has EnemyAI
        if (other.gameObject.GetComponent<EnemyAI>())
        {
            Debug.Log("Hit!");
        }
    }
}
