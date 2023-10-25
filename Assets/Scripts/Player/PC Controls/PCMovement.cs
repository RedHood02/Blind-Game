using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PCMovement : MonoBehaviour
{
    [SerializeField] float walkingSpeed, runningSpeed, tiptoingSpeed;
    float z, x;
    [SerializeField] bool isRunning, isTiptoing;

    //Fmod
    [SerializeField] StudioEventEmitter emitter;


    [SerializeField] float timeToNextStepMaster, timeToNextStep;

    //Double tap
    [SerializeField] float doubleTapTimer;


    [SerializeField] GameObject terrainScannerPrefab, terrainScannerWater;

    public bool canMove;
    [SerializeField] bool inWater, isMoving;

    [SerializeField] Vector3 lastPos;

    public int currentState;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        lastPos = transform.position;
    }
    private void Update()
    {
        GetAxis();
        MovePlayer();
        CheckPos();
        BoolControl();
        Clap();
    }

    void MovePlayer()
    {
        if (canMove)
        {
            if (!isRunning && !isTiptoing)
            {
                currentState = 1;
                Debug.Log("Moving");
                transform.Translate(walkingSpeed * Time.deltaTime * new Vector3(x, 0, z));
                if (isMoving)
                {
                    Walking();
                }
            }

            if (isRunning)
            {
                currentState = 2;
                transform.Translate(walkingSpeed * Time.deltaTime * new Vector3(x, 0, z));
                if (isMoving)
                {
                    Running();
                }
            }

            if (isTiptoing)
            {
                currentState = 0;
                transform.Translate(walkingSpeed * Time.deltaTime * new Vector3(x, 0, z));
            }
        }
    }

    void CheckPos()
    {
        if (transform.position != lastPos)
        {
            isMoving = true;

        }
        else
        {
            isMoving = false;
            timeToNextStep = timeToNextStepMaster;
        }

        lastPos = transform.position;
    }

    void GetAxis()
    {
        z = Input.GetAxisRaw("Vertical");
        x = Input.GetAxisRaw("Horizontal");
    }


    void BoolControl()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isRunning = false;
            isTiptoing = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isTiptoing = false;
        }
    }

    void Walking()
    {
        timeToNextStep -= Time.deltaTime;
        Debug.Log("Lowering");
        if (timeToNextStep <= 0)
        {
            RuntimeManager.PlayOneShot("event:/Player/Walking");

            if (inWater)
            {
                GetComponent<TerrainScanner>().SpawnScanner(terrainScannerWater);
            }
            else
            {
                Debug.Log("Entered");
                GetComponent<TerrainScanner>().SpawnScanner(terrainScannerPrefab);
                timeToNextStep = timeToNextStepMaster;
            }
        }
    }

    void Running()
    {
        timeToNextStep -= Time.deltaTime * 2f;
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
                timeToNextStep = timeToNextStepMaster;
            }
        }
    }

    void Clap()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RuntimeManager.PlayOneShot("event:/Player/Hand Slap");
            GetComponent<TerrainScanner>().SpawnScannerClap(terrainScannerPrefab);
        }
    }
}
