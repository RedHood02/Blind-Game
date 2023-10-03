using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerEnemyDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemySM>().SetHunt();
        }
    }
}
