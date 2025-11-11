using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    //take all player input regardless of gamestate
    
    #region Input
    
    public InputActionAsset inputActions;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _dashAction; //dash instead of sprint??
    
    #endregion
    #region Variables
    #region Public
    [Header("Vertical Movement")]
    [SerializeField, Tooltip ("EXTRA downward force applied to the player (Project Gravity: -100)")]
    private float gravityForce;
    [SerializeField, Tooltip ("How hard the player jumps against gravity")]
    private float jumpForce = 40;
    [SerializeField, Tooltip ("How fast the player climbs up/down poles")]
    private float climbSpeed = 35;
    
    [Header("Horizontal Movement")]
    [SerializeField, Tooltip ("How fast the player walks")]
    private float walkSpeed = 55;
    [SerializeField, Tooltip ("How fast the player moves mid-dash")]
    private float dashSpeed = 85;
    #endregion Public
    #region Local
    
    private Rigidbody _rb;
    private float _moveSpeed;
    private Vector2 _moveAmount;
    
    #endregion Local
    #endregion Variables
    #region Flags
    
    private bool IsGrounded { get; set; }
    private bool IsWalking { get; set; }
    private bool IsDashing { get; set; }
    private bool IsClimbing { get; set; }
    
    #endregion
    #region Awake/OnEnable/OnDisable

    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
    }
    private void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();
    }
    private void Awake()
    {
        _moveAction = inputActions.FindAction("Main/Move");
        _jumpAction = inputActions.FindAction("Main/Jump");
        _dashAction = inputActions.FindAction("Main/Dash");
        _rb = GetComponent<Rigidbody>();
        _moveSpeed = walkSpeed;
    }
    
    #endregion
    #region Update | FixedUpdate
    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        CheckInput();
        Walk();
        PlayerVelocityReduction();
        //PlayerVelocityDirection();
    }
    #endregion
    #region Unity Functions

    private void CheckInput()
    {
        _moveAmount = _moveAction.ReadValue<Vector2>(); // Walk & Climb
        _moveSpeed = IsDashing ? dashSpeed : walkSpeed;
    }
    private Vector3 GetDirection() => Mathf.Abs(_moveAmount.x) > 0.001f
        ? (_moveAmount.x > 0 ? Vector3.right : Vector3.left)
        : Vector3.zero;

    #endregion Unity Functions
    #region Movement
    private void Walk() => _rb.MovePosition(Time.fixedDeltaTime * _moveAmount.x * _moveSpeed * transform.right + _rb.position);
    private void OnJump() => _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    private void Climb() => _rb.MovePosition(Time.fixedDeltaTime * _moveAmount.y * climbSpeed * transform.up + _rb.position);
    private void OnDash()  => _rb.AddForce(GetDirection() * dashSpeed, ForceMode.Impulse);

    private void PlayerVelocityReduction()
    {
        Vector3 vel = _rb.linearVelocity;
        float excess = Mathf.Abs(vel.x) - walkSpeed;
        if (excess > 0f)
        {
            float sign = Mathf.Sign(vel.x);
            Vector3 counter = new Vector3(-sign * excess * 1f, 0f, 0f);
            _rb.AddForce(counter, ForceMode.VelocityChange);
        }
    }
    private void PlayerVelocityDirection()
    {
        Vector3 dir = _moveAmount.normalized;
        _rb.linearVelocity = new Vector3(dir.x, 0, 0) * _moveSpeed;
    }
    
    #endregion Movement

}
