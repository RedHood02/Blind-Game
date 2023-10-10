using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainScanner : MonoBehaviour
{

    [SerializeField] float duration, size;


    // Update is called once per frame

    public void SpawnScanner(GameObject prefabToSpawn)
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
}
