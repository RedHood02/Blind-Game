using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField] Transform playerTransform;
	Vector3 offset;


	private void Start()
	{
		offset = transform.position - playerTransform.position;
	}

	private void Update()
	{
		transform.position = playerTransform.position + offset;
	}
}
