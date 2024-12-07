using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon { get; private set; }

    PlayerControls playerControls;

    bool attackButtonDown, isAttacking = false;

    protected override void Awake() 
    {
        base.Awake();

        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.Enable(); // Enable player controls
    }

    void Start()
    {
        // This line of code lets us quickly execute the Attack()
        // method without the need for a separate handler because 
        // the Attack() method doesn't require a parameter. 

        // When mouse is held down we start swinging
        playerControls.Combat.Attack.started += _ => StartAttacking();
        // When mouse button is lifted, we stop. 
        playerControls.Combat.Attack.canceled += _ => StopAttacking();
    }

    void Update() 
    {
        Attack();
    }

    // Set our new weapon to the current active weapon
    public void NewWeapon(MonoBehaviour newWeapon)
    {
        CurrentActiveWeapon = newWeapon;
    }

    // All of this attacking logic is the same as it was in our sword script. 
    // The difference is that now we're using an interface script 'IWeapon' to 
    // hand generalized attacking regardless of weapon. 
    public void ToggleIsAttacking(bool value)
    {
        isAttacking = value;
    }

    void StartAttacking()
    {
        attackButtonDown = true;
    }

    void StopAttacking()
    {
        attackButtonDown = false;
    }

    // Now in this attack function, whatever weapon is set as our currently active weapon 
    // will have the 'Attack' method from that specific weapon called. 
    void Attack()
    {
        if (attackButtonDown && !isAttacking)
        {
            isAttacking = true;
            (CurrentActiveWeapon as IWeapon).Attack();
        }
        
    }
}
