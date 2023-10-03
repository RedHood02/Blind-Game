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
    public List<GameObject> terrainScannerPrefab = new();
    public float duration, size;
    public float timeToNextStepMaster, timeToNextStep;

    //Enemy AI
    public NavMeshAgent agent;
    public Vector3 playerPos;
    public float timerToReset, timerMaster;
    public bool heardPlayer;
    public List<Transform> restingSpotsList = new();
    public float radius;


    //Change To Rest
    public float restTimerMaster, currentTimer;


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
                heardPlayer = true;
                break;

            case "Resting":
                break;

            case "Hunting":
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

    public void SpawnScanner(int id)
    {
        GameObject terrainScanner = Instantiate(terrainScannerPrefab[id], gameObject.transform.position, Quaternion.identity);

        if (terrainScanner.transform.GetChild(0).TryGetComponent<ParticleSystem>(out var terrainScannerPs))
        {
            var main = terrainScannerPs.main;
            main.startLifetime = duration;
            main.startSize = size;
        }

        Destroy(terrainScanner, duration + 1);
    }



    public bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    public void IncreaseRestTimer(float valueAdded)
    {
        currentTimer += valueAdded;
    }
}
