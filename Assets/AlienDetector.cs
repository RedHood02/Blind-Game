using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemySM>().inArea = true;
        }
    }
}
