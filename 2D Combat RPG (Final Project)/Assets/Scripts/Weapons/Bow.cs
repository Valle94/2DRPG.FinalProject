using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] WeaponInfo weaponInfo;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] Transform arrowSpawnPoint;

    Animator myAnimator;

    // This readonly hash is a simple performance hack. By hashing
    // our string, it decreases the amount of memory used every time
    // we have to pass that value through a function. In this case,
    // it's every time we fire the bow, so in theory a lot.
    readonly int FIRE_HASH = Animator.StringToHash("Fire");

    void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    // The unique attack method to the bow. 
    public void Attack()
    {
        // Start the bow attack animation
        myAnimator.SetTrigger(FIRE_HASH);

        // Instantiate an arrow. Because My sprites are offset by 45 degrees,
        // I have to do some funky stuff to make everything face the correct
        // direction in game. You'll see something similar in Projectile.cs
        GameObject newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, 
                                        Quaternion.Euler(0f, 0f, -45f) * ActiveWeapon.Instance.transform.rotation);
        newArrow.GetComponent<Projectile>().UpdateWeaponInfo(weaponInfo);
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}
