using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // This public property can be called in other classes which allows us to 
    // set our private bool facingLeft from outside of this class.
    public bool FacingLeft { get { return facingLeft; } set { facingLeft = value; } }

    [SerializeField] float moveSpeed = 1f;

    PlayerControls playerControls; //Player controls based on built-in Unity Input System
    Vector2 movement;
    Rigidbody2D rb;
    Animator myAnimator; //Handles animating the player sprite
    SpriteRenderer mySpriteRenderer;

    bool facingLeft = false;

    // Initializing various components in Awake
    void Awake() 
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
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
            FacingLeft = true;
        }
        else
        {
            mySpriteRenderer.flipX = false;
            FacingLeft = false;
        }
    }
}