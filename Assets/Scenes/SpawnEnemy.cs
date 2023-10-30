using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject emitter;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            emitter.GetComponent<StudioEventEmitter>().Play();
            enemy.SetActive(true);
        }
    }
}
