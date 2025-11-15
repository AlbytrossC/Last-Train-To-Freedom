using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Public Variables
    
    public enum EnemyType { Idle, Active }

    public EnemyType myType;
    public float walkSpeed = 40; //player = 80f
    public float rotationSpeed; //idle = 30, active = 100
    public float maxWalkDist = 150;
    [Header("IDLE ONLY STATS")] 
    public float maxTurnLeft = 45;
    public float maxTurnRight = 135;
    
    #endregion
    #region Private Variables
    
    private float currentWalkSpeed;
    private Vector3 currentRotationAngle {get; set;}
    private float desiredRotationAngle;
    private int turnDir = 1;
    private Rigidbody _rb;
    private Vector3 startPos;
    private float walkDist = 0;
    
    #endregion
    #region Unity Methods

    private void Start()
    {
        SetupEnemy(myType);
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        switch (myType)
        {
            case EnemyType.Active:
                FixedActive();
                break;
            case EnemyType.Idle:
                FixedIdle();
                break;
        }
    }

    #endregion
    #region Behaviour

    private void SetupEnemy(EnemyType t)
    {
        startPos = transform.position;
        currentRotationAngle = transform.eulerAngles;
        switch (t)
        {
            case EnemyType.Idle:
                rotationSpeed = 30;
                desiredRotationAngle = 90; //temp to make face camera
                break;
            case EnemyType.Active:
                currentWalkSpeed = walkSpeed;
                rotationSpeed = 100;
                break;
        }
    }

    private void FixedIdle()
    {
        desiredRotationAngle += turnDir;
        RotateEnemy(Turn(desiredRotationAngle));
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, Turn(desiredRotationAngle+turnDir), transform.eulerAngles.z);
        if (transform.eulerAngles.y < maxTurnLeft)
        {
            RotateEnemy(maxTurnLeft + 1);
            turnDir *= -1;
        }
        if (transform.eulerAngles.y > maxTurnRight)
        {
            RotateEnemy(maxTurnRight - 1);
            turnDir *= -1;
        }
    }

    private void FixedActive()
    {
        Walk();
    }

    private float Turn(float dir)
    {
        var curAngle = transform.eulerAngles;
        return Mathf.LerpAngle(curAngle.y, dir, rotationSpeed * Time.deltaTime);
    }

    private void Walk()
    {
        walkDist += MoveEnemy().x;
        if (Math.Abs(walkDist) > maxWalkDist)
        {
            print(RotateEnemy(transform.eulerAngles.y + 180));
            //RotateEnemy();
            walkSpeed *= -1;
            walkDist = 0;
        }
    }
    
    #endregion
    
    #region Helpers

    private Vector3 RotateEnemy(float rotation = 90)
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotation, transform.eulerAngles.z);
        return transform.eulerAngles;
    }

    private Vector3 MoveEnemy() //returns distance moved that frame
    {
        var oldPos = transform.position;
        transform.Translate(Vector3.right * walkSpeed * Time.deltaTime, Space.World);
        return transform.position - oldPos;
    }
    
    #endregion
}
