using System;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionAsset InputActions;
    private InputAction iMoveAction;
    private InputAction iJumpAction;
    private InputAction iRunAction;
    
    private Vector2 moveAmount;
    private Rigidbody rb;
    public float walkSpeed = 50;
    public float jumpForce = 40;
    public float climbSpeed = 50;
    private bool isGrounded = true;
    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }
    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        iMoveAction = InputActions.FindAction("Player/Move");
        iJumpAction = InputActions.FindAction("Player/Jump");
        iRunAction = InputActions.FindAction("Player/Run");
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        moveAmount = iMoveAction.ReadValue<Vector2>();
        if (iJumpAction.WasPressedThisFrame())
        {
            if (isGrounded)
                Jump();
        }
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    private void OnCollisionStay(Collision other)
    {
        isGrounded = true;
    }

    private void FixedUpdate()
    {
        Walking();
        Climbing();
    }

    private void Walking()
    {
        rb.MovePosition(rb.position + transform.forward * moveAmount.x * walkSpeed * Time.deltaTime);
    }
    private void Climbing()
    {
        rb.AddForce(transform.up * moveAmount.y * climbSpeed * Time.deltaTime, ForceMode.Force);
    }
}
