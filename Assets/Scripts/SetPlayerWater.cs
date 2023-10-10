using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SetPlayerWater : MonoBehaviour
{
	[SerializeField] GameObject waterPrefab;

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			other.GetComponent<Movement>().SetInWater(true);
			other.GetComponent<TerrainScanner>().SpawnScanner(waterPrefab);
		}
	}


	private void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			other.GetComponent<Movement>().SetInWater(false);
		}
	}
}
