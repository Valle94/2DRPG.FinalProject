using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletMoveSpeed;
    [SerializeField] int burstCount;
    [SerializeField] int projectilesPerBurst;
    [SerializeField][Range(0, 359)] float angleSpread;
    [SerializeField] float startingDistance = 0.1f;
    [SerializeField] float timeBetweenBursts;
    [SerializeField] float restTime = 1f;
    [SerializeField] bool stagger;
    [Tooltip("Stagger has to be enabled for oscillate to function properly.")]
    [SerializeField] bool oscillate;
    
    bool isShooting = false;

    // This validation method isn't called in game, but rather in the
    // Unity inspector. It lets us set values in the inspector to be
    // something valid, in the event we accidentally type something wrong
    void OnValidate() 
    {
        if (oscillate) {stagger = true; }
        if (!oscillate) {stagger = false; }
        if (projectilesPerBurst < 1) {projectilesPerBurst = 1; }
        if (burstCount < 1) {burstCount = 1; }
        if (timeBetweenBursts < 0.1f) { timeBetweenBursts = 0.1f; }
        if (restTime < 0.1f) { restTime = 0.1f; }
        if (startingDistance < 0.1f) { startingDistance = 0.1f; }
        if (angleSpread == 0) { projectilesPerBurst = 1;}
        if (bulletMoveSpeed <= 0) {bulletMoveSpeed = 0.1f; }
    }

    // This attack method shoots bullets towards the player
    public void Attack()
    {
        if (!isShooting)
        {
            StartCoroutine(ShootRoutine());
        }
    }

    IEnumerator ShootRoutine()
    {
        isShooting = true;

        float startAngle, currentAngle, angleStep, endAngle;
        float timeBetweenProjectiles = 0f;

        // Call our method which defines our cone
        TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);

        // If we want to stagger our shots (not fire simultaneously)
        if (stagger)
        {
            timeBetweenProjectiles = timeBetweenBursts / projectilesPerBurst;
        }

        // This for loop handles a burst of shots by the enemy
        for (int i = 0; i < burstCount; i++)
        {
            // If we aren't oscillating, get a new cone every burst
            if (!oscillate)
            {
                // Redefine our cone of influence between bursts
                // so when the player moves, the cone shoots towards
                // the new position
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            }

            // If we are oscillating we get a new cone every other burst
            if (oscillate && i % 2 != 1)
            {
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            }
            else if (oscillate)
            {
                // If we are oscillating, we're basically just reversing everything
                // related to our cone, so the bullets can shoot in the opposite direction
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStep *= -1;
            }

            // This nested for loop handles each shot in that burst
            for (int j = 0; j < projectilesPerBurst; j++)
            {
                Vector2 pos = FindBulletSpawnPos(currentAngle);

                //Instantiate the bullet from our prefab
                GameObject newBullet = Instantiate(bulletPrefab, pos, Quaternion.identity);

                // Move the bullet towards the player position at time of firing
                newBullet.transform.right = newBullet.transform.position - transform.position;

                // This new syntax lets us get a projectile component of our new 
                // bullet and sumultaneously call and return variables to the
                // specific instance of that class.
                if (newBullet.TryGetComponent(out Projectile projectile))
                {
                    projectile.UpdateMoveSpeed(bulletMoveSpeed);
                }

                // Increment to the next step in our bullet cone.
                currentAngle += angleStep;

                // If we are staggering our shots, wait before firing the next one
                if (stagger)
                {
                    yield return new WaitForSeconds(timeBetweenProjectiles);
                }
            }

            //After each burst, set current angle back to starting angle
            currentAngle = startAngle;

            if (!stagger)
            {
                yield return new WaitForSeconds(timeBetweenBursts);
            }
        }


        yield return new WaitForSeconds(restTime);
        isShooting = false;
    }

    void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, out float endAngle)
    {
        // Set the target direction based on player transform position
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;

        // This group of properties are all used to define a bullet cone
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        startAngle = targetAngle;
        endAngle = targetAngle;
        currentAngle = targetAngle;
        float halfAngleSpread = 0f;
        angleStep = 0;

        // This if statement basically reads if we want a cone, and not to
        // just shoot in a straight line. The angle spread will be defined 
        // in the inspector, and is the total angle for our bullet cone.
        if (angleSpread != 0)
        {
            // This angle step is how many degrees are between
            // each bullet in our firing arc.
            angleStep = angleSpread / (projectilesPerBurst - 1);

            halfAngleSpread = angleSpread / 2f;

            // These angles represent the the angle created
            // by each edge of our bullet cone
            startAngle = targetAngle - halfAngleSpread;
            endAngle = targetAngle + halfAngleSpread;

            // The first bullet of our first burst should start
            // at the far edge of our cone
            currentAngle = startAngle;
        }
    }

    // This method takes in our current angle as defined above
    // and returns the point (x,y) along the line startingDistance
    // from our starting position and current angle
    Vector2 FindBulletSpawnPos(float currentAngle)
    {
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);
        
        Vector2 pos = new Vector2(x, y);

        return pos;
    }
}
