using System.Collections;
using UnityEngine;
using UnityEngine.Events;
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
    public Transform wallCheck;
    public LayerMask wallLayer;
    public Collider2D attackCollider;
    public Collider2D hitBox;

    public UnityEvent attackEvent;

    private InputAction move;
    private InputAction jump;
    private InputAction fire;
    private bool isFacingRight = true;
    private bool hasDoubleJumped = false;
    private bool canAttack = true;
    
    public float dashDistance = 7f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;
    private bool canDash = true;
    private bool isDashing = false;
    private InputAction dash;
    
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.5f;
    private bool isKnockedBack = false;
    public GameObject pivotEnemy;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
        
        dash = playerControls.Player.Dash;
        
        dash.performed += Dash;
    }
    
    private void OnEnable()
    {
        playerControls.Player.Enable();
    }

    private void OnDisable()
    {
        playerControls.Player.Disable();
    }

    public void FixedUpdate()
    {
        if (!isDashing && !isKnockedBack)
        {
            float speedMultiplier =
                IsGrounded() ? 1f : 0.8f; // Use full speed on the ground, halve the speed in the air
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
            hasDoubleJumped = true; // Set the flag to indicate that the player has used their double jump
        }

        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private bool IsGrounded()
    {
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer) || Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);

        if (isGrounded)
        {
            hasDoubleJumped = false; // Reset the double jump flag when the player touches the ground
        }

        return isGrounded;
    }

    private bool CanAttack()
    {
        bool canAttack = false;
        return canAttack;
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

    public void Attack(InputAction.CallbackContext context)
    {
        
        if (context.performed && canAttack)
        {
            attackCollider.gameObject.SetActive(true);
            attackEvent.Invoke();
            canAttack = false;
            Debug.Log($"Player position: {transform.position.x}, Enemy position: {pivotEnemy.transform.position.x}");
           
            StartCoroutine(attackCooldown());

        }
        if (context.canceled)
        {
            attackCollider.gameObject.SetActive(false);

        }
        
        
    }

    IEnumerator attackCooldown()
    {
        yield return new WaitForSeconds(.4f);
        canAttack = true;
        
    }
    
    private void Dash(InputAction.CallbackContext context)
    {
        if (canDash && !isDashing)
        {
            StartCoroutine(PerformDash());
        }
    }

    private IEnumerator PerformDash()
    {
        canDash = false;
        isDashing = true;
        hasDoubleJumped = false;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;

        hitBox.enabled = false;

        Vector2 dashDirection = isFacingRight ? Vector2.right : Vector2.left;
        Vector2 dashVelocity = dashDirection * (dashDistance / dashDuration);

        float dashTimer = 0;
        while (dashTimer < dashDuration)
        {
            rb.velocity = dashVelocity;
            dashTimer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.gravityScale = originalGravity;
        isDashing = false;

        hitBox.enabled = true;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    
    public void Knockback(Transform pivotEnemy)
    {
        if (!isKnockedBack)
        {
            StartCoroutine(PerformKnockback(pivotEnemy));
        }
    }

    private IEnumerator PerformKnockback( Transform pivotEnemy)
    {
        yield return new WaitForSeconds(.45f);
        isKnockedBack = true;
       
        playerControls.Player.Disable();
        
        Debug.Log($"Player position: {transform.position.x}, Enemy position: {pivotEnemy.transform.position.x}");

        float knockbackDirection;
        if (transform.position.x < pivotEnemy.transform.position.x)
        {
            knockbackDirection = -1f;
        }
        else
        {
            knockbackDirection = 1f; 
        }
        
        Vector2 knockbackVelocity = new Vector2(knockbackForce * knockbackDirection, rb.velocity.y);
        rb.velocity = knockbackVelocity;
        
        yield return new WaitForSeconds(knockbackDuration);
        
        playerControls.Player.Enable();
        
        isKnockedBack = false;
    }
}
