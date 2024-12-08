using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float moveSpeed = 22f;
    [SerializeField] GameObject particleOnHitPrefabVFX;
    [SerializeField] bool isEnemyProjectile = false;
    [SerializeField] float projectileRange = 10f;
    
    Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }

    public void UpdateProjectileRange(float projectileRange)
    {
        this.projectileRange = projectileRange;
    }

    public void UpdateMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    // This method handles various triggers related to when the projectile
    // hits different objects
    void OnTriggerEnter2D(Collider2D other)
    {
        // When triggered, get components of the object being hit
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        Indestructable indestructable = other.gameObject.GetComponent<Indestructable>();
        PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();

        // If the other object isn't a trigger and has either enemy health
        // or our empty 'indestructable' script,
        if (!other.isTrigger && (enemyHealth || indestructable || player))
        {
            if ((player && isEnemyProjectile) || (enemyHealth && !isEnemyProjectile))
            {
                player?.TakeDamage(1, transform);
                // Create a visual effect
                Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
                // Destroy the arrow
                Destroy(gameObject);
            }
            else if (!other.isTrigger && indestructable)
            {
                // Create a visual effect
                Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
                // Destroy the arrow
                Destroy(gameObject);                
            }
        }
    }

    // This method destroys our projectile once it reaches the max range
    // of the weapon or enemy that fired it. 
    void DetectFireDistance()
    {
        if (Vector3.Distance(transform.position, startPosition) > projectileRange)
        {
            Destroy(gameObject);
        }
    }

    // This method makes the arrow actually fly through the air
    // by adjusting the transform in the direction the arrow is 
    // facing when it's instantiated. 
    void MoveProjectile()
    {
        // Correct the local movement direction
        Vector3 correctedDirection = Quaternion.Euler(0f, 0f, 45f) * Vector3.right;

        if (!isEnemyProjectile)
        {
        // Move the projectile in the corrected direction
        transform.Translate(correctedDirection * Time.deltaTime * moveSpeed, Space.Self);
        }
        else
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        }
    }
}
