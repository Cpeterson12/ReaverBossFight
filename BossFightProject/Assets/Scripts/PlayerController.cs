using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public FloatData moveSpeed;
    public FloatData horizontal;
    public FloatData jumpSpeed;
    public PlayerInputActions playerControls;
    public Transform groundCheck;
    public LayerMask groundLayer;


    private InputAction move;
    private InputAction jump;
    private bool isFacingRight = true;
    private bool hasDoubleJumped = false;


    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    public void Update()
    {
        float speedMultiplier = IsGrounded() ? 1f : 0.7f; // Use full speed on the ground, halve the speed in the air
        rb.velocity = new Vector2(horizontal.data * moveSpeed.data * speedMultiplier, rb.velocity.y);

        if (!isFacingRight && horizontal.data > 0f)
        {
            Flip();
        }
        else if (isFacingRight && horizontal.data < 0f)
        {
            Flip();
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed.data);
        }

        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }
    
    public void DoubleJump(InputAction.CallbackContext context)
    {
        if (context.performed && !IsGrounded() && !hasDoubleJumped)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed.data);
            hasDoubleJumped = true;
        }

        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private bool IsGrounded()
    {
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (isGrounded)
        {
            hasDoubleJumped = false;
        }

        return isGrounded;

    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal.data = context.ReadValue<Vector2>().x;
    }

    
}
