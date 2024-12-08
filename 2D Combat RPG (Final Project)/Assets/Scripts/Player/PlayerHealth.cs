using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 3;
    [SerializeField] float knockBackThrustAmount = 10f;
    [SerializeField] float damageRecoveryTime = 1f;

    int currentHealth;
    bool canTakeDamage = true;

    Knockback knockback;
    Flash flash;

    void Awake()
    {
        knockback = GetComponent<Knockback>();
        flash =  GetComponent<Flash>();
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    // The collision method is similar to ontriggerenter2d, except
    // it checks every frame for a collision, not just the moment of contact
    void OnCollisionStay2D(Collision2D other)
    {
        // Get enemy ai component (if it exists)
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        // If it does exist and we haven't take damage recently
        if (enemy)
        {   
            // Take Damage
            TakeDamage(1, other.transform);
        }
    }
    
    // This method applies the damage to our current health and sets
    // canTakeDamage to false to create 'invincibility frames' 
    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage) 
        {
            return;
        }
        
        // Get knocked back
        knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        // Flash the player
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());
    }

    IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }
}
