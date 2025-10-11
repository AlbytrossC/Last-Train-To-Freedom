using System;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionAsset InputActions;
    
    private InputAction iMoveAction;
    private InputAction iJumpAction;

    private Vector2 moveAmount;
    private Rigidbody rb;
    
    public float walkSpeed = 5;
    public float jumpSpeed = 5;
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
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        moveAmount = iMoveAction.ReadValue<Vector2>();
        print(moveAmount);
        if (iJumpAction.WasPressedThisFrame())
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.AddForceAtPosition(new Vector3(0, jumpSpeed, 0), Vector3.up, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        Walking();
    }

    private void Walking()
    {
        rb.MovePosition(rb.position + transform.forward * moveAmount.x * walkSpeed * Time.deltaTime);
    }
}
