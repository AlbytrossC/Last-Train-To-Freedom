using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionAsset InputActions;
    private InputAction iMoveAction;
    private InputAction iJumpAction;
    private InputAction iRunAction;

    public int currentPoleID;
    private Vector2 moveAmount;
    private Rigidbody rb;
    public float walkSpeed = 50;
    public float runSpeed = 65;
    private float moveSpeed;
    public float jumpForce = 40;
    public float climbSpeed = 50;
    private bool isGrounded = true;
    public bool isClimbing = false;
    private bool canClimb = false;
    public bool startClimb = false;
    
    public Camera cam1;
    public Camera cam2;
    public TMP_Text camHUDtxt;
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
        iRunAction = InputActions.FindAction("Player/Sprint");
        rb = GetComponent<Rigidbody>();
        moveSpeed = walkSpeed;
    }

    public void SetCurrentPoleID(int id)
    {
        currentPoleID = id;
    }
    void Update()
    {
        moveAmount = iMoveAction.ReadValue<Vector2>();
        if (iJumpAction.WasPressedThisFrame())
        {
            if (isGrounded)
                Jump();
        }
        if (Input.GetKeyDown(KeyCode.C)) {
            cam1.enabled = !cam1.enabled;
            cam2.enabled = !cam2.enabled;
            print("Switching Camera");
            camHUDtxt.text = Camera.main.gameObject.name;
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
        
        rb.MovePosition(rb.position + transform.forward * moveAmount.x * moveSpeed * Time.deltaTime);
        moveSpeed = iRunAction.IsPressed() ? runSpeed : walkSpeed;
        Camera.main.fieldOfView = iRunAction.IsPressed() ? Mathf.Lerp(65, 60, 0.1f) : Mathf.Lerp(60, 65, 0.1f);

        // if (iRunAction.IsPressed())
        // {
        //     Camera.main.fieldOfView = Mathf.Lerp(65, 60, 0.1f);
        // }
    }

    // private IEnumerator AnimateFOVChange(bool sprinting)
    // {
    //     int val = 0;
    //     switch (sprinting)
    //     {
    //         case true:
    //             val = 10;
    //             break;
    //         case false:
    //             val = -10;
    //             break;
    //     }
    //     yield return new WaitForSeconds(0.1f);
    //     mainCamera.fieldOfView += val;
    //     yield return new WaitForSeconds(0.1f);
    //     mainCamera.fieldOfView += val;
    //     yield return new WaitForSeconds(0.1f);
    //     mainCamera.fieldOfView += val;
    //     print("Changed FOV");
    // }
    private void Climbing()
    {
        switch (isClimbing)
        {
            case true: rb.MovePosition(rb.position + transform.up * moveAmount.y * climbSpeed * Time.deltaTime);
                break;
            case false:
                if (canClimb)
                {
                    if (moveAmount.y != 0)
                    {
                        startClimb = true;
                    }
                }
                break;
        }
    }

    public void TogglePlayerClimb()
    {
        isClimbing = !isClimbing;
        switch (isClimbing) 
        {
            case true:
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                break;
            case false: 
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                break;
        }
    }

    public void AllowToggleClimb() => canClimb = true;
    public void DenyToggleClimb() => canClimb = false;
}
