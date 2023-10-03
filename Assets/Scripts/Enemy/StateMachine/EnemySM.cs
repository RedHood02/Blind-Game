using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySM : StateMachine
{
    [HideInInspector]
    public Idle idleState;
    [HideInInspector]
    public Resting restingState;
    [HideInInspector]
    public Hunting huntingState;

    //Enemy Visual Effect
    public GameObject terrainScannerPrefab;
    public float duration, size;
    public float timeToNextStepMaster, timeToNextStep;

    //Enemy AI
    public NavMeshAgent agent;
    public float enemySpeed;
    public Vector3 playerPos;
    public float timerToReset, timerMaster;
    public bool heardPlayer;

    private void Awake()
    {
        idleState = new Idle(this);
        restingState = new Resting(this);
        huntingState = new Hunting(this);   
    }

    public void SetHunt()
    {
        string currentState = FindObjectOfType<StateMachine>().GetCurrentState().ToString();
        switch(currentState)
        {
            case "Idle":
                Debug.Log("Enemy Idle");
                heardPlayer = true;
                break;

            case "Resting":
                Debug.Log("Enemy Resting");
                break;

            case "Hunting":
                Debug.Log("Enemy Hunting");
                huntingState.ResetTimer();
                break;

            default:
                break;
        }
    }

    protected override BaseState GetInitialState()
    {
        return idleState;
    }

    public void SpawnScanner()
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
