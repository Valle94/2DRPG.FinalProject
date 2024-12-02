using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{

    [SerializeField] int damageAmount = 1;

    // This script is attached to our weapon collider 'hitbox'
    void OnTriggerEnter2D(Collider2D other) 
    {
        // Call the TakeDamage script from EnemyHealth to apply the damageAmount
        // in this script to the health amount in that script
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        enemyHealth?.TakeDamage(damageAmount);
    }
}
