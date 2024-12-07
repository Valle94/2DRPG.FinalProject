using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    // This public property can be called in other classes which allows us to 
    // set our private bool facingLeft from outside of this class.
    public bool FacingLeft { get { return facingLeft; } }

    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float dashSpeed = 4f;
    [SerializeField] TrailRenderer myTrailRenderer;
    [SerializeField] Transform slashAnimSpawnPoint;
    [SerializeField] Transform weaponCollider;

    PlayerControls playerControls; //Player controls based on built-in Unity Input System
    Vector2 movement;
    Rigidbody2D rb;
    Animator myAnimator; //Handles animating the player sprite
    SpriteRenderer mySpriteRenderer;
    float startingMoveSpeed;

    bool facingLeft = false;
    bool isDashing = false;

    // Initializing various components in Awake
    protected override void Awake() 
    {
        // Call inheriting awake first
        base.Awake();

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start() 
    {
        // Subscribing to the dash event when the dash button is pressed
        playerControls.Combat.Dash.performed += _ => Dash();
        startingMoveSpeed = moveSpeed;
    }

    // Enabling the player controls script used by the input system
    private void OnEnable() 
    {
        playerControls.Enable();
    }

    // Update occurs once per frame, but because that call is not 
    // frame rate independent, it's not actually a regular update interval
    void Update()
    {
        PlayerInput();
    }

    // FixedUpdate is called at a measured step based on the physics engine
    // and so is called at a regular interval. 
    void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
    }

    public Transform GetWeaponCollider()
    {
        return weaponCollider;
    }

    public Transform GetSlashAnimSpawnPoint()
    {
        return slashAnimSpawnPoint;
    }

    // This method alters a variable 'movement' based on user inputs
    // and sets float fields in the animator based on that variable.
    void PlayerInput()
    {
        // The playercontrols script which handles the input system returns
        // a Vector2 when input is registered, which is then read and stored here
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        // This sets the corresponding floats in the animator to a value
        // which will change our sprite's animation state.
        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    // This method handles moving the rigidbody2d attached to the player
    void Move()
    {
        // We move position based on our current position, 'movement' 
        // calculated in PlayerInput() which determines direction,
        // and our moveSpeed. We use fixedDeltaTime because this method
        // is being called in FixedUpdate.
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    // This method is used to turn the player character's
    // sprite to face the mouse at all times
    void AdjustPlayerFacingDirection()
    {
        // First store the mouse position as a variable
        Vector3 mousePos = Input.mousePosition;
        // Then store the player position as a variable by converting world
        // position to a position relative to the camera view (screen space).
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        // If-else that flips the sprite horizontally depending on
        // relative position to the mouse.
        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRenderer.flipX = true;
            facingLeft = true;
        }
        else
        {
            mySpriteRenderer.flipX = false;
            facingLeft = false;
        }
    }

    // This method handles player 'dashing'
    void Dash()
    {
        // If we aren't dashing
        if (!isDashing)
        {
            // Set us to dashing
            isDashing = true;
            // Increase speed
            moveSpeed *= dashSpeed;
            // Render Particle Effects
            myTrailRenderer.emitting = true;
            // Run Dash ending corouting
            StartCoroutine(EndDashRoutine());
        }
    }

    // This corouting handles both how long the dash lasts
    // as well as how often we can dash
    IEnumerator EndDashRoutine()
    {
        float dashTime = 0.2f;
        float dashCD = 0.25f;
        // After a certain amount of time, return speed to normal
        yield return new WaitForSeconds(dashTime);
        moveSpeed = startingMoveSpeed;
        // Turn off particle effects
        myTrailRenderer.emitting = false;
        // Set dashing cooldown
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }
}