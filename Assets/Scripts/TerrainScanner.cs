using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainScanner : MonoBehaviour
{
    [SerializeField] GameObject terrainScannerPrefab;
    [SerializeField] float duration, size;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnScanner();
        }
    }

    void SpawnScanner()
    {
        GameObject terrainScanner = Instantiate(terrainScannerPrefab, gameObject.transform.position, Quaternion.identity);
        
        if (terrainScanner.transform.GetChild(0).TryGetComponent<ParticleSystem>(out var terrainScannerPs))
        {
            var main = terrainScannerPs.main;
            main.startLifetime = duration;
            main.startSize = size;
        }

        Destroy(terrainScanner, duration + 1);
    }
}
