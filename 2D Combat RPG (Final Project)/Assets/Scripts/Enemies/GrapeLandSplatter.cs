using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeLandSplatter : MonoBehaviour
{
    SpriteFade spriteFade;

    void Awake()
    {
        spriteFade = GetComponent<SpriteFade>();
    }

    void Start()
    {
        // Once the splatter is spawned, fade it out
        StartCoroutine(spriteFade.SlowFadeRoutine());

        // Disable the collider after a shorter amount of time
        Invoke("DisableCollider", 0.2f);
    }

    // If our player touches the splatter, take damage and get knocked back
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        playerHealth?.TakeDamage(1, transform);
    }

    void DisableCollider()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
    }
}
