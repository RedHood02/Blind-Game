using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Movement : MonoBehaviour
{
    // References
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CharacterController characterController;

    // Player settings
    [SerializeField] private float cameraSensitivity;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveInputDeadZone;

    // Touch detection
    private int leftFingerId, rightFingerId;
    private float halfScreenWidth;

    // Camera control
    private Vector2 lookInput;
    private float cameraPitch;

    // Player movement
    private Vector2 moveTouchStartPosition;
    private Vector2 moveInput;

    //Fmod
    [SerializeField] StudioEventEmitter emitter;


    [SerializeField] float currentTouchDistance;
    [SerializeField] float timeToNextStepMaster, timeToNextStep;

    //Double tap
    [SerializeField] float doubleTapTimer;


    [SerializeField] GameObject terrainScannerPrefab, terrainScannerWater;

    [SerializeField] bool inWater;
    public bool canMove;

    //Movement states
    public float currentState; // 0 = tiptoing, 1 = walking, 2 = running
    [SerializeField] float tiptoingSpeed, walkingSpeed, runningSpeed;

    //Raycast
    [SerializeField] LayerMask pipeMask;

    void Start()
    {

        // id = -1 means the finger is not being tracked
        leftFingerId = -1;
        rightFingerId = -1;

        // only calculate once
        halfScreenWidth = Screen.width / 2;

        // calculate the movement input dead zone
        moveInputDeadZone = Mathf.Pow(Screen.height / moveInputDeadZone, 2);

        timeToNextStep = timeToNextStepMaster;

    }

    // Update is called once per frame
    void Update()
    {
        // Handles input
        GetTouchInput();


        if (rightFingerId != -1 && canMove)
        {
            // Ony look around if the right finger is being tracked
            LookAround();
        }

        if (leftFingerId != -1 && canMove)
        {
            // Ony move if the left finger is being tracked
            Move();
        }
    }


    void DoubleTapAction()
    {
        RuntimeManager.PlayOneShot("event:/Player/Hand Slap");
        GetComponent<TerrainScanner>().SpawnScannerClap(terrainScannerPrefab);
        doubleTapTimer = 0.3f;
    }

    void GetTouchInput()
    {
        doubleTapTimer -= Time.deltaTime;
        // Iterate through all the detected touches
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);
            if (t.tapCount == 2 && doubleTapTimer < 0)
            {
                DoubleTapAction();
            }

            if (t.phase == TouchPhase.Began)
            {
                Debug.Log("Casted");
                CastRaycast();
            }

            // Check each touch's phase
            switch (t.phase)
            {
                case TouchPhase.Began:

                    if (t.position.x < halfScreenWidth && leftFingerId == -1)
                    {
                        // Start tracking the left finger if it was not previously being tracked
                        leftFingerId = t.fingerId;

                        // Set the start position for the movement control finger
                        moveTouchStartPosition = t.position;
                    }
                    else if (t.position.x > halfScreenWidth && rightFingerId == -1)
                    {
                        // Start tracking the rightfinger if it was not previously being tracked
                        rightFingerId = t.fingerId;
                    }

                    break;
                case TouchPhase.Ended:

                    timeToNextStep = timeToNextStepMaster;

                    if (t.fingerId == leftFingerId)
                    {
                        // Stop tracking the left finger
                        leftFingerId = -1;
                        currentTouchDistance = 0;
                    }
                    else if (t.fingerId == rightFingerId)
                    {
                        // Stop tracking the right finger
                        rightFingerId = -1;
                    }

                    break;
                case TouchPhase.Canceled:

                    timeToNextStep = timeToNextStepMaster;
                    if (t.fingerId == leftFingerId)
                    {
                        // Stop tracking the left finger
                        leftFingerId = -1; currentTouchDistance = 0;
                    }
                    else if (t.fingerId == rightFingerId)
                    {
                        // Stop tracking the right finger
                        rightFingerId = -1;
                    }

                    break;
                case TouchPhase.Moved:

                    Vector3 tPosition = Input.GetTouch(0).position;

                    float number = Vector3.Distance(moveTouchStartPosition, tPosition);

                    currentTouchDistance = number;

                    // Get input for looking around
                    if (t.fingerId == rightFingerId)
                    {
                        lookInput = cameraSensitivity * Time.deltaTime * t.deltaPosition;
                    }
                    else if (t.fingerId == leftFingerId)
                    {

                        // calculating the position delta from the start position
                        moveInput = t.position - moveTouchStartPosition;
                    }

                    break;
                case TouchPhase.Stationary:
                    // Set the look input to zero if the finger is still
                    if (t.fingerId == rightFingerId)
                    {
                        lookInput = Vector2.zero;
                    }
                    break;
            }
        }
    }



    void CastRaycast()
    {
        if (Physics.Raycast(Camera.main.gameObject.transform.position, transform.forward, out RaycastHit hit, 10, pipeMask))
        {
            Debug.Log(hit.collider.name);
            if (hit.collider.GetComponent<WaterPipe>().GetIsInRange() == true)
            {
                hit.collider.GetComponent<WaterPipe>().ActivatePipe();
            }
            return;
        }
        return;
    }
    void LookAround()
    {

        // vertical (pitch) rotation
        /*
        cameraPitch = Mathf.Clamp(cameraPitch - lookInput.y, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);
        */
        // horizontal (yaw) rotation
        transform.Rotate(transform.up, lookInput.x);
    }

    void Move()
    {
        Vector2 moveDir = Vector2.zero;
        if (currentTouchDistance > 0 && currentTouchDistance <= 25)
        {
            Tiptoing(out moveDir);
        }
        else if (currentTouchDistance > 25 && currentTouchDistance <= 65)
        {
            Walking(out moveDir);
        }
        else if (currentTouchDistance > 65)
        {
            Running(out moveDir);
        }

        Vector3 vect = new(moveDir.x, 0, moveDir.y);


        transform.Translate(vect);

    }

    public void SetInWater(bool newBool)
    {
        inWater = newBool;
    }

    void Tiptoing(out Vector2 movementDirection)
    {
        currentState = 0;
        moveSpeed = tiptoingSpeed;
        timeToNextStep = timeToNextStepMaster;

        if (moveInput.sqrMagnitude <= moveInputDeadZone)
        {
            movementDirection = Vector2.zero;
        }
        else
        {
            movementDirection = moveSpeed * Time.deltaTime * moveInput.normalized;
        }

    }

    void Walking(out Vector2 movementDirection)
    {
        currentState = 1;
        moveSpeed = walkingSpeed;

        if (moveInput.sqrMagnitude <= moveInputDeadZone)
        {
            movementDirection = Vector2.zero;
        }
        else
        {
            movementDirection = moveSpeed * Time.deltaTime * moveInput.normalized;
        }


        timeToNextStep -= Time.deltaTime;
        if (timeToNextStep <= 0)
        {
            RuntimeManager.PlayOneShot("event:/Player/Walking");

            if (inWater)
            {
                GetComponent<TerrainScanner>().SpawnScanner(terrainScannerWater);
            }
            else
            {
                GetComponent<TerrainScanner>().SpawnScanner(terrainScannerPrefab);
            }
            timeToNextStep = timeToNextStepMaster;
        }
    }

    void Running(out Vector2 movementDirection)
    {
        currentState = 2;
        moveSpeed = runningSpeed;

        if (moveInput.sqrMagnitude <= moveInputDeadZone)
        {
            movementDirection = Vector2.zero;
        }
        else
        {
            movementDirection = moveSpeed * Time.deltaTime * moveInput.normalized;
        }


        timeToNextStep -= Time.deltaTime * 1.75f;
        if (timeToNextStep <= 0)
        {
            RuntimeManager.PlayOneShot("event:/Player/Walking");

            if (inWater)
            {
                GetComponent<TerrainScanner>().SpawnScanner(terrainScannerWater);
            }
            else
            {
                GetComponent<TerrainScanner>().SpawnScanner(terrainScannerPrefab);
            }
            timeToNextStep = timeToNextStepMaster;
        }
    }
}
