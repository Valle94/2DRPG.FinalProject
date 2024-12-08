using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{
    [SerializeField] WeaponInfo weaponInfo;
    [SerializeField] GameObject magicLaser;
    [SerializeField] Transform magicLaserSpawnPoint;

    Animator myAnimator;

    // This readonly hash is a simple performance hack. By hashing
    // our string, it decreases the amount of memory used every time
    // we have to pass that value through a function. In this case,
    // it's every time we fire the laser, so in theory a lot.
    readonly int ATTACK_HASH = Animator.StringToHash("Attack");

    void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    void Update() 
    {
        MouseFollowWilthOffset();
    }
    public void Attack()
    {
        myAnimator.SetTrigger(ATTACK_HASH);
    }

    // This method is called at the end of the staff
    // attack animation events
    public void SpawnStaffProjectileAnimEvent()
    {
        // Instantiate the laser 
        GameObject newLaser = Instantiate(magicLaser, magicLaserSpawnPoint.position, Quaternion.identity);
        // Update the laser range of that specific laser instance
        newLaser.GetComponent<MagicLaser>().UpdateLaserRange(weaponInfo.weaponRange);
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    void MouseFollowWilthOffset()
    {
        // First store the mouse position as a variable
        Vector3 mousePos = Input.mousePosition;
        // Then store the player position as a variable by converting world
        // position to a position relative to the camera view (screen space).
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        // We're generating an angle using Mathf functions; Atan2 gives a 
        // result in radians so we convert to degrees using Rad2Deg.
        float angle = Mathf.Atan2(mousePos.y - playerScreenPoint.y, 
                      Mathf.Abs(mousePos.x - playerScreenPoint.x)) * Mathf.Rad2Deg;

        // If-else that flips the sprite horizontally depending on
        // relative position to the mouse. The angle calculated above
        // is used to rotate the sword to follow the mouse.
        if (mousePos.x < playerScreenPoint.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
            // This line flips our weaponCollider too
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
