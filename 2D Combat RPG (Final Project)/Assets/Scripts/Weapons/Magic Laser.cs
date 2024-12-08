using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLaser : MonoBehaviour
{
    [SerializeField] float laserGrowTime = 2f;

    bool isGrowing = true;
    float laserRange;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider2D;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    void Start()
    {
        LaserFaceMouse();
    }

    // This trigger stops our laser from growing when we hit
    // walls or trees (or anything that's 'indestructable').
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.GetComponent<Indestructable>() && !other.isTrigger)
        {
            isGrowing = false;
        }
    }

    // This method simply takes our langer range as defined in the 
    // scriptable object field 'weapon range' and assigns it to 
    // the local variable 'laser range'
    public void UpdateLaserRange(float laserRange)
    {
        this.laserRange = laserRange;
        StartCoroutine(IncreaseLaserLengthRoutine());
    }

    // This coroutine grows both the visual, and the capsule collider
    // attached to an instantiated laser over time.
    IEnumerator IncreaseLaserLengthRoutine()
    {
        float timePassed = 0f;

        // While our laser has not yet reached it's maximum range 
        // and it hasn't hit an object that turns isGrowing to false
        while (spriteRenderer.size.x < laserRange && isGrowing)
        {   
            // Increase time and generate a time variable that factors
            // in how quickly we want our laser to 'grow'
            timePassed += Time.deltaTime;
            float linearT = timePassed / laserGrowTime;

            // Increase the size of the sprite in the x direction only
            // once again using Lerp to grow steadily instead of instantly
            spriteRenderer.size = new Vector2(Mathf.Lerp(1f, laserRange, linearT), 1f);

            // Same thing with the capsule collider, only in this case because
            // it expands outward in both directions from the center, we both
            // need to increas the size and the offset, so it only grows in 
            // the direction we are firing.
            capsuleCollider2D.size = new Vector2(Mathf.Lerp(1f, laserRange, linearT), capsuleCollider2D.size.y);
            capsuleCollider2D.offset = new Vector2 ((Mathf.Lerp(1f, laserRange, linearT)) / 2, 0f);

            yield return null;
        }

        // After the laser is finished doing its thing, fade it out
        StartCoroutine(GetComponent<SpriteFade>().SlowFadeRoutine());
    }

    // This method simply faces the laser in the direction the mouse
    // is pointing. It's called in start, so it happens when the laser
    // is instantiated. 
    void LaserFaceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = transform.position - mousePosition;

        transform.right = -direction;        
    }
}
