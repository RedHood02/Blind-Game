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


		if (rightFingerId != -1)
		{
			// Ony look around if the right finger is being tracked
			LookAround();
		}

		if (leftFingerId != -1)
		{
			// Ony move if the left finger is being tracked
			Move();
		}
	}


	void DoubleTapAction()
	{
		RuntimeManager.PlayOneShot("event:/Player/Hand Slap");
		GetComponent<TerrainScanner>().SpawnScanner(terrainScannerPrefab);
		doubleTapTimer = 0.1f;
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
						leftFingerId = -1;
					}
					else if (t.fingerId == rightFingerId)
					{
						// Stop tracking the right finger
						rightFingerId = -1;
					}

					break;
				case TouchPhase.Moved:

					float number = Vector3.Distance(moveTouchStartPosition, t.position);

					currentTouchDistance = Mathf.Clamp(number, 0, 3);

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

		// Don't move if the touch delta is shorter than the designated dead zone
		if (moveInput.sqrMagnitude <= moveInputDeadZone) return;

		// Multiply the normalized direction by the speed
		Vector2 movementDirection = moveSpeed * Time.deltaTime * moveInput.normalized;
		// Move relatively to the local transform's direction

		Vector3 vect = new(movementDirection.x, 0, movementDirection.y);


		transform.Translate(vect);

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

	public void SetInWater(bool newBool)
	{
		inWater = newBool;
	}
}
