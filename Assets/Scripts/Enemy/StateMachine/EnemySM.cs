using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FMODUnity;
using Cinemachine;

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
    public Light enemyLight;

    //Mesh
    public MeshRenderer meshRend;

    //Animator
    public Animator anim;

    //FMOD
    public StudioEventEmitter emitter, roarEmitter;

    //Cinemachine
    public CinemachineImpulseSource source;

    //SFX
    [SerializeField] float timer;

    private void Awake()
    {
        idleState = new Idle(this);
        restingState = new Resting(this);
        huntingState = new Hunting(this);
    }

    public void SetHunt()
    {
        string currentState = FindObjectOfType<StateMachine>().GetCurrentState().ToString();
        switch (currentState)
        {
            case "Idle":
                heardPlayer = true;
                break;

            case "Hunting":
                huntingState.ResetTimer();
                break;

            default:
                break;
        }
    }

    public void RandomiseSound()
    {
        string currentState = FindObjectOfType<StateMachine>().GetCurrentState().ToString();
        if (currentState == "Idle")
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                int rand = Random.Range(0, 2);
                if (rand == 0)
                {
                    agent.isStopped = true;
                    roarEmitter.Play();
                }
                else
                {
                    return;
                }
                agent.isStopped = false;
                timer = 10f;
            }
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

    public void DirectorTeleport(Vector3 firstPipe, Vector3 secondPipe)
    {

    }

    public void DirectorTip(Vector3 newPos)
    {

    }

    public void DirectorLeave(Vector3 newPos)
    {
        
    }

    public void DirectorRest()
    {
        ChangeState(restingState);
    }

    public void DirectorAwake()
    {

    }

    void a()
    {
       
    }

    public void FirstSpawn()
    {
        gameObject.SetActive(true);
    }


    public void ChangeAnimation()
    {
        meshRend.enabled = false;
        anim.Play("New State");
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
