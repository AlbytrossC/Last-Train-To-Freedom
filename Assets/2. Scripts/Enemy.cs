using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    #region Public Variables
    
    public enum EnemyType { Idle, Active, Window }

    public GameObject flashlight;
    public GameObject blinds = null;
    public EnemyType myType;
    public bool windowOddTimer = false;
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
    private bool isThinking = false;
    
    #endregion
    #region Unity Methods

    private void Start()
    {
        SetupEnemy(myType);
    }

    private void FixedUpdate()
    {
        if (isThinking) return;
        switch (myType)
        {
            case EnemyType.Active:
                FixedActive();
                break;
            case EnemyType.Idle:
                FixedIdle();
                break;
            case EnemyType.Window:
                FixedWindow();
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
                desiredRotationAngle = 90;
                StartCoroutine(ThinkAboutIt());
                break;
            case EnemyType.Active:
                currentWalkSpeed = walkSpeed;
                rotationSpeed = 100;
                StartCoroutine(ThinkAboutIt());
                break;
            case EnemyType.Window:
                desiredRotationAngle = 90;
                if (windowOddTimer)
                {
                    StartCoroutine(ThinkAboutIt(false, true));
                }
                break;
        }
    }

    private void FixedIdle()
    {
        desiredRotationAngle += turnDir;
        RotateEnemy(Turn(desiredRotationAngle));
        if (transform.eulerAngles.y < maxTurnLeft)
        {
            StartCoroutine(ThinkAboutIt());
            RotateEnemy(maxTurnLeft + 1);
            turnDir *= -1;
        }
        if (transform.eulerAngles.y > maxTurnRight)
        {
            StartCoroutine(ThinkAboutIt());
            RotateEnemy(maxTurnRight - 1);
            turnDir *= -1;
        }
    }

    private void FixedWindow()
    {
        StartCoroutine(ThinkAboutIt(false, true));
        flashlight.SetActive(!flashlight.activeSelf);
        blinds.SetActive(!flashlight.activeSelf);
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
            StartCoroutine(ThinkAboutIt(true));
            //RotateEnemy(transform.eulerAngles.y + 180);
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

    private IEnumerator ThinkAboutIt(bool EnemyTurnFix = false, bool windowfix = false)
    {
        var clr = GetComponent<Renderer>().material.color;
        isThinking = true;
        GetComponent<Renderer>().material.color = Color.blueViolet;

        if (windowfix)
        {
            yield return new WaitForSeconds(2);
            isThinking = false;
            GetComponent<Renderer>().material.color = clr;
            yield break;
        }
        
        if (Random.Range(0, 100) < 50)
            yield return new WaitForSeconds(Random.Range(0.1f, 1.5f));
        else
            yield return new WaitForSeconds(Random.Range(3.0f, 5.0f));

        isThinking = false;
        GetComponent<Renderer>().material.color = clr;
        if (EnemyTurnFix) RotateEnemy(transform.eulerAngles.y + 180);
        
    }

    #endregion
}
