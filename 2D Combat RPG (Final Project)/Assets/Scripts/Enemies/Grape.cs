using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grape : MonoBehaviour, IEnemy
{
    [SerializeField] GameObject grapeProjectilePrefab;

    Animator myAnimator;
    SpriteRenderer spriteRenderer;

    readonly int ATTACK_HASH = Animator.StringToHash("Attack");

    void Awake()
    {
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // The grape specific attack method
    public void Attack()
    {   
        // Start the attack animation
        myAnimator.SetTrigger(ATTACK_HASH);

        // Face the grape enemy towards the player
        if (transform.position.x - PlayerController.Instance.transform.position.x < 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    // This method instantiates the grape projectile and will
    // be called at the end of the attack animation as an event.
    public void SpawnProjectileAnimEvent()
    {
        Instantiate(grapeProjectilePrefab, transform.position, Quaternion.identity);
    }
}
