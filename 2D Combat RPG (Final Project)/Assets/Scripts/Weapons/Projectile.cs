using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float moveSpeed = 22f;
    [SerializeField] GameObject particleOnHitPrefabVFX;

    WeaponInfo weaponInfo;
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

    public void UpdateWeaponInfo(WeaponInfo weaponInfo)
    {
        this.weaponInfo = weaponInfo;
    }

    // This method handles various triggers related to when the arrow
    // hits different objects
    void OnTriggerEnter2D(Collider2D other)
    {
        // When triggered, get components of the object being hit
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        Indestructable indestructable = other.gameObject.GetComponent<Indestructable>();
        
        // If the other object isn't a trigger and has either enemy health
        // or our empty 'indestructable' script,
        if (!other.isTrigger && (enemyHealth || indestructable))
        {
            // Create a visual effect
            Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
            // Destroy the arrow
            Destroy(gameObject);
        }
    }

    void DetectFireDistance()
    {
        if (Vector3.Distance(transform.position, startPosition) > weaponInfo.weaponRange)
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

        // Move the projectile in the corrected direction
        transform.Translate(correctedDirection * Time.deltaTime * moveSpeed, Space.Self);
    }
}
