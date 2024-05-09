using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public FloatData moveSpeed;
    public FloatData jumpSpeed;
    public PlayerInputActions playerControls;
 
    private Vector2 moveDirection = Vector2.zero;
    private Vector2 jumpDirection = Vector2.zero;
    private InputAction move;
    private InputAction jump;


    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        jump = playerControls.Player.Jump;
        move.Enable();
        jump.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }

    public void Update()
{
    moveDirection = move.ReadValue<Vector2>();
    jumpDirection = jump.ReadValue<Vector2>();

}

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed.data, jumpDirection.y * jumpSpeed.data);
    }
}
