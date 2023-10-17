using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainScanner : MonoBehaviour
{

    public float walkingDuration, walkingSize, runningDuration, runningSize;


    // Update is called once per frame

    public void SpawnScanner(GameObject prefabToSpawn)
    {
        GameObject terrainScanner = Instantiate(prefabToSpawn, gameObject.transform.position, Quaternion.identity);

        if (terrainScanner.transform.GetChild(0).TryGetComponent<ParticleSystem>(out var terrainScannerPs) && terrainScanner.name == "Scanner(Clone)" && terrainScanner.transform.GetChild(0).TryGetComponent<SphereCollider>(out var sphereCollider))
        {
            var main = terrainScannerPs.main;
            if (GetComponent<Movement>().currentState == 1)
            {
                main.startLifetime = walkingDuration;
                main.startSize = walkingSize;
                sphereCollider.radius = 50;
            }
            else if (GetComponent<Movement>().currentState == 2)
            {
                main.startLifetime = runningDuration;
                main.startSize = runningSize;
                sphereCollider.radius = 125;
            }
            Destroy(terrainScanner, walkingDuration + 1);
        }
        else if (terrainScanner.name == "ScannerWater(Clone)" && terrainScanner.transform.GetChild(0).TryGetComponent<SphereCollider>(out var sphere))
        {
            var main = terrainScannerPs.main;
            if (GetComponent<Movement>().currentState == 1)
            {
                main.startLifetime = walkingDuration;
                main.startSize = walkingSize;
                sphere.radius = 150f;
            }
            else if (GetComponent<Movement>().currentState == 2)
            {
                main.startLifetime = runningDuration;
                main.startSize = runningSize;
                sphere.radius = 200f;
            }
            Destroy(terrainScanner, walkingDuration + 1);
        }
    }

    public void SpawnScannerClap(GameObject prefabToSpawn)
    {
        GameObject terrainScanner = Instantiate(prefabToSpawn, gameObject.transform.position, Quaternion.identity);

        if (terrainScanner.transform.GetChild(0).TryGetComponent<ParticleSystem>(out var terrainScannerPs) && terrainScanner.name == "Scanner(Clone)" && terrainScanner.transform.GetChild(0).TryGetComponent<SphereCollider>(out var sphereCollider))
        {
            var main = terrainScannerPs.main;
            main.startLifetime = walkingDuration;
            main.startSize = walkingSize;
            sphereCollider.radius = 50;

        }

        Destroy(terrainScanner, walkingDuration + 1);
    }
}
