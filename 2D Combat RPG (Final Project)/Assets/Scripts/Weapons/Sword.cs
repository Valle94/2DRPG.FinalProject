using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] GameObject slashAnimPrefab;
    [SerializeField] WeaponInfo weaponInfo;
    
    Transform weaponCollider;
    Transform slashAnimSpawnPoint;
    Animator myAnimator;

    GameObject slashAnim;

    // Initialize scripts and components in Awake
    void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        weaponCollider = PlayerController.Instance.GetWeaponCollider();
        slashAnimSpawnPoint = PlayerController.Instance.GetSlashAnimSpawnPoint();
    }

    void Update()
    {
        MouseFollowWilthOffset();
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    // This method holds all the actions we want to execute when
    // the player attacks with the sword.
    public void Attack()
    {
        // This SetTrigger animates our sword swing
        myAnimator.SetTrigger("Attack");

        // When we swing our sword, turn on the sword weapon collider
        weaponCollider.gameObject.SetActive(true);

        // Here we are instantiating the sword swinging particle
        // effects at a predefined spawn point
        slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
        // This sets the instantiated objects parent in the hierarchy;
        // Because the sword's parent is ActiveWeapon, it will also 
        // set the instantiated effects parent to be ActiveWeapon
        slashAnim.transform.parent = this.transform.parent;
    }

    // This method is called from our sword animator; it turns off
    // the sword collider after the sword is done swinging.
    public void DoneAttackingAnimEvent()
    {
        weaponCollider.gameObject.SetActive(false);
    }

    // This method handles changing the animation for the sword swing animation.
    // It is called in the animator using events.
    public void SwingUpFlipAnimEvent()
    {
        // This line reverses the slash particle for the upward swing
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

        // If we are facing left as determined in the player controller, flip
        // this whole animation to the left side of the player.
        if (PlayerController.Instance.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    // This method is similar to the one above
    public void SwingDownFlipAnimEvent()
    {
        // This line reverses the slash particle for the upward swing
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        // If we are facing left as determined in the player controller, flip
        // this whole animation to the left side of the player.
        if (PlayerController.Instance.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    void MouseFollowWilthOffset()
    {
        // First store the mouse position as a variable
        Vector3 mousePos = Mouse.current.position.ReadValue();
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
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
