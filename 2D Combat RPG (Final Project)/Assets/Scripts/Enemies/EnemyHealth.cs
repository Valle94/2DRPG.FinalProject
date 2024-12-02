using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int startingHealth = 3;
    [SerializeField] GameObject deathVFXPrefab;

    int currentHealth;
    Knockback knockback;
    Flash flash;

    void Awake() 
    {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

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
        knockback.GetKnockedBack(PlayerController.Instance.transform, 15f);
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(CheckDetectDeathRoutine());
    }

    private IEnumerator CheckDetectDeathRoutine()
    {
        yield return new WaitForSeconds(flash.GetRestoreMatTime());
        DetectDeath();
    }

    // This method checks to see if the HP is <=0
    // If so, it destroys the enemy.
    void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
