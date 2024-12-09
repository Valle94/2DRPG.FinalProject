using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Pickup : MonoBehaviour
{
    enum PickUpType
    {
        GoldCoin,
        StaminaGlobe,
        HealthGlobe,
    }

    [SerializeField] PickUpType pickUpType;
    [SerializeField] float pickUpDistance = 5f;
    [SerializeField] float accelerationRate = 0.2f;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] AnimationCurve animCurve;
    [SerializeField] float heightY = 1.5f;
    [SerializeField] float popDuration = 1f;

    Vector3 moveDir;
    Rigidbody2D rb;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        StartCoroutine(AnimCurveSpawnRoutine());
    }

    private void Update()
    {
        // Constantly update our player position
        Vector3 playerPos = PlayerController.Instance.transform.position;

        // If the player is close enough to the pickup, set the move direction
        // and speed variables constantly towards the player
        if (Vector3.Distance(transform.position, playerPos) < pickUpDistance)
        {
            moveDir = (playerPos - transform.position).normalized;
            moveSpeed += accelerationRate * Time.deltaTime;
        }
        // Else leave the pickup object as stationary
        else
        {
            moveDir = Vector3.zero;
            moveSpeed = 0f;
        }
    }

    void FixedUpdate()
    {
        // Move the rigidbody of the pickup based on the direction and movespeed
        rb.velocity = moveDir * moveSpeed * Time.fixedDeltaTime;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        // When the pickup collides with the player
        if (other.gameObject.GetComponent<PlayerController>())
        {
            // Apply the effect of that pickup
            DetectPickupType();
            // Destroy it
            Destroy(gameObject);
        }
    }

    // Just like with the grape projectile, we're using an animation curve
    // when we instantiate our pickups so they spawn in a little random arc
    IEnumerator AnimCurveSpawnRoutine()
    {
        Vector2 startPoint = transform.position;
        float randomX = transform.position.x + Random.Range(-2f, 2f);
        float randomY = transform.position.y + Random.Range(-1f, 1f);

        Vector2 endPoint = new Vector2(randomX, randomY);

        float timePassed = 0f;

        while (timePassed < popDuration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / popDuration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, heightY, heightT);

            transform.position = Vector2.Lerp(startPoint, endPoint, linearT) + new Vector2(0f, height);
            
            yield return null;
        }
    }

    // When this method is called when we pick up an item, it will 
    // determine what type of item we picked up and execute those actions
    void DetectPickupType()
    {
        // Once again, this switch is basically a bunch of if-then statements
        switch(pickUpType)
        {
            case PickUpType.GoldCoin:
                EconomyManager.Instance.UpdateCurrentGold();
                break;
            case PickUpType.HealthGlobe:
                PlayerHealth.Instance.HealPlayer();
                break;
            case PickUpType.StaminaGlobe:
                Stamina.Instance.RefreshStamina();
                break;
            default:
                break;
        }
    }
}
