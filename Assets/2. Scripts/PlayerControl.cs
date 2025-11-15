using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    #region Public Variables
    public enum PlayerState { idle, walking, dashing, falling, climbing }
    public PlayerState playerState = PlayerState.idle;
    
    [Header("Horizontal Movement")]
    [Tooltip("Walking speed")]
    public float walkSpeed = 80f;

    [Header("Dash")]
    [Tooltip("Speed while dashing (units per second)")]
    public float dashSpeed = 180f;
    [Tooltip("How long the dash lasts (seconds).")]
    public float dashDuration = 0.18f;
    [Tooltip("Seconds between dashes.")]
    public float dashCooldown = 0.6f;
    [Tooltip("How fast player speed eases back to walk speed after dash speed ends.")]
    [Range(1f, 100f)]
    public float dashRecoveryRate = 8f;

    [Header("Jump")]
    [Tooltip("Impulse Force applied when jumping.")]
    public float jumpForce = 110f;

    [Header("Climb")] 
    [Tooltip("Climb Speed = Walk Speed * [Climb Multiplier]")]
    public float climbSpeedMultiplier = 0.7f;
    public bool isOnLadder;
    public float ladderExitForce = 60f;

    [Header("Ground Check")]
    public LayerMask groundMask;
    public Transform groundCheck;
    public Vector3 halfWidth = new Vector3(4f, 2f, 4f);

    [Header("Extra Gravity (optional)")]
    [Tooltip("Additional downward force ontop of gravity (-100)")]
    public float extraGravityForce = 0f;

    [Header("Input")]
    public InputActionAsset actionsAsset;
    #endregion
    #region Private Variables
    
    private Rigidbody _rb;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _dashAction;

    private Vector2 _moveInput;   // X = left/right Y = up/down 
    private bool _isGrounded;
    private bool _isDashing;
    [SerializeField] private bool _canClimb;
    private float _dashTimer;
    private Vector3 _dashDirection;
    private float _lastDashTime = -Mathf.Infinity;
    private float TargetSpeed => walkSpeed;
    #endregion
    #region Public Methods

    public void ToggleCanClimb(bool state) => _canClimb = state;

    public void LeaveLadder()
    {
        print(_rb.linearVelocity.x);
        if (!isOnLadder) return;
        isOnLadder = false;
        var pos = 1;
        if (_moveInput.y < 0) pos = -1;
        _rb.AddForce(new Vector3(_rb.linearVelocity.x, (jumpForce/2) * pos, _rb.linearVelocity.z), ForceMode.Impulse);
    } 
    
    #endregion
    #region Unity Methods
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        _rb.freezeRotation = true;
        
        var playerMap = actionsAsset.FindActionMap("Main");
        _moveAction = playerMap.FindAction("Move");
        _jumpAction = playerMap.FindAction("Jump");
        _dashAction = playerMap.FindAction("Dash");
        
        _jumpAction.performed += ctx => Jump();
        _dashAction.performed += ctx => StartDash();
        _moveAction.performed += ctx => CheckMove(ctx);
    }
    private void OnEnable()
    {
        _moveAction.Enable();
        _jumpAction.Enable();
        _dashAction.Enable();
    }
    private void OnDisable()
    {
        _moveAction.Disable();
        _jumpAction.Disable();
        _dashAction.Disable();
    }
    private void Update()
    {
        _moveInput = _moveAction.ReadValue<Vector2>();
        _moveInput.x = Math.Sign(_moveInput.x);
        _moveInput.y = Math.Sign(_moveInput.y);
        _rb.useGravity = !isOnLadder;
    }
    private void FixedUpdate()
    {
        StateManager();
        GroundCheck();
        GravityCheck();
        Move();
        Climb();
    }

    #endregion
    #region Movement & Mechanics
    private void GroundCheck() => _isGrounded = Physics.CheckBox(
        groundCheck.position,
        halfWidth,
        Quaternion.identity,                                 
        groundMask, QueryTriggerInteraction.Ignore);

    private void CheckMove(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValue<Vector2>().y == 0) return;
        //isOnLadder = _canClimb;
    }
    private void Move()
    {
        if (_isDashing)
        {
            Vector3 vel = _rb.linearVelocity;
            vel.x = _dashDirection.x * dashSpeed;
            _rb.linearVelocity = vel;

            _dashTimer -= Time.fixedDeltaTime;
            if (_dashTimer <= 0f)
            {
                _isDashing = false;
            }
        }
        else
        {
            HorizontalMovement();
        }

        if (_moveInput.y != 0) isOnLadder = _canClimb;
    }
    private void GravityCheck()
    {
        if (isOnLadder) return;
        _rb.AddForce(Vector3.down * extraGravityForce, ForceMode.Force);
    }
    private void Jump()
    {
        if (!_isGrounded) return;
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        LeaveLadder();
    }
    private void StartDash()
    {
        // Prevent new dash while dashing or if dash on cooldown
        if (_isDashing) return;
        if (Time.time - _lastDashTime < dashCooldown) return;

        // Set dash direction based on input direction
        // If no input, dash right
        float horiz = _moveInput.x;
        _dashDirection = Mathf.Abs(horiz) > 0.1f
            ? (horiz > 0f ? Vector3.right : Vector3.left)
            : Vector3.right;
        
        _isDashing   = true;
        _dashTimer   = dashDuration;
        _lastDashTime = Time.time;
    }
    private void HorizontalMovement()
    {
        float desiredX = _moveInput.x * walkSpeed;
        Vector3 vel = _rb.linearVelocity;
        vel.x = Mathf.Lerp(vel.x, desiredX, 1f - Mathf.Exp(-dashRecoveryRate * Time.fixedDeltaTime));
        _rb.linearVelocity = vel;
    }
    private void Climb()
    {
        if (!isOnLadder) return;
        LadderStall();
        float climbSpeed = _moveInput.y * (walkSpeed * climbSpeedMultiplier);
        _rb.MovePosition(transform.position + Vector3.up * climbSpeed * Time.fixedDeltaTime);    
    }

    private void LadderStall()
    {
        Vector3 vel = _rb.linearVelocity;
        vel.y = 0;
        _rb.linearVelocity = vel;
    }
    #endregion
    #region Debug
    
    private void StateManager()
    {
        switch (playerState)
        {
            case PlayerState.idle:
                break;
            case PlayerState.walking:
                break;
            case PlayerState.dashing:
                break;
            case PlayerState.climbing:
                break;
            case PlayerState.falling:
                break;
            
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(groundCheck.position, halfWidth * 2);
        }
    }
    
    #endregion
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}