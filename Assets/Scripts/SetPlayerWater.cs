using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SetPlayerWater : MonoBehaviour
{
    [SerializeField] GameObject waterPrefab;
    [SerializeField] StudioEventEmitter emitter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Movement>().SetInWater(true);

            if (other.GetComponent<Movement>().currentState == 1 || other.GetComponent<Movement>().currentState == 2)
            {
                other.GetComponent<TerrainScanner>().SpawnScanner(waterPrefab);
                emitter.Play();
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Movement>().SetInWater(false);
            emitter.Play();
        }
    }
}
