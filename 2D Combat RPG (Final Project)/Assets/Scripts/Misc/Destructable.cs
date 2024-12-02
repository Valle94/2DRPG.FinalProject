using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField] GameObject destroyVFX;
    
    
    void OnTriggerEnter2D(Collider2D other) 
    {
        // When the sword swings and hits the grass
        if (other.gameObject.GetComponent<DamageSource>())
        {
            // Create the particle effects for destroying the grass
            Instantiate(destroyVFX, transform.position, Quaternion.identity);
            Destroy(gameObject); // Destroy the grass
        }
    }
}
