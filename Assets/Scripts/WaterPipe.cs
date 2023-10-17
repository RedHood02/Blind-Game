using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPipe : MonoBehaviour
{

    [SerializeField] bool isActive, playerInRange;
    [SerializeField] GameObject scannerPrefab;
    [SerializeField] float waitTime, duration, size;

    public void ActivatePipe()
    {
        if (!isActive)
        {
            Debug.Log("Cor started");
            StartCoroutine(Pipe());
            isActive = true;
        }
    }

    IEnumerator Pipe()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            SpawnScanner(scannerPrefab);
        }
    }

    void SpawnScanner(GameObject prefabToSpawn)
    {
        GameObject terrainScanner = Instantiate(prefabToSpawn, gameObject.transform.position, Quaternion.identity);

        if (terrainScanner.transform.GetChild(0).TryGetComponent<ParticleSystem>(out var terrainScannerPs))
        {
            var main = terrainScannerPs.main;
            main.startLifetime = duration;
            main.startSize = size;
        }
        Destroy(terrainScanner, duration + 1);
    }

    public void SetIsInRange(bool newBool)
    {
        playerInRange = newBool;
    }
    public bool GetIsInRange()
    {
        return playerInRange;
    }
}
