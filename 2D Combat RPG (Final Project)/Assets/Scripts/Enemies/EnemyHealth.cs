using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int startingHealth = 3;

    int currentHealth;

    void Start() 
    {
        // Initialize current health to be the same as starting health
        currentHealth = startingHealth;
    }

    // This method will be callable from other scripts, mainly the damage
    // sources script, which will pass in different damage numbers depending
    // on the weapon being used
    public void TakeDamage(int damage)
    {
        // Subtract the damage from the current health
        currentHealth -= damage;
        Debug.Log(currentHealth);
        // Check to see if the enemy is 'dead'
        DetectDeath();
    }

    // This method checks to see if the HP is <=0
    // If so, it destroys the enemy.
    void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
