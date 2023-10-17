using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePlayerDetecter : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponentInChildren<WaterPipe>().SetIsInRange(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponentInChildren<WaterPipe>().SetIsInRange(false);
        }
    }
}
