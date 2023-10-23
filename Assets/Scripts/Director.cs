using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    public static Director _Director;

    public List<GameObject> checkpoints = new(), alienPipe = new(), waterPipe = new();

    public EnemySM enemyStateMachine;

    public Transform enemyTransform, playerTransform;


    public float timer;
    //Venting
    public Transform selectedPipe;

    private void Awake()
    {
        _Director = this;

        enemyStateMachine = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemySM>();
    }


    private void Update()
    {
        EnemyTimer();
    }


    void EnemyTimer()
    {
        if (enemyStateMachine.GetCurrentState().ToString() == "Idle")
        {
            timer += Time.deltaTime;
        }

        if (enemyStateMachine.GetCurrentState().ToString() == "Hunting")
        {
            timer += Time.deltaTime * 2;
        }


        if (timer > 120 && enemyStateMachine.GetCurrentState().ToString() != "Hunting" && enemyStateMachine.GetCurrentState().ToString() != "Venting")
        {
            enemyStateMachine.ChangeState(enemyStateMachine.ventingState);
        }
    }

    public void Vent()
    {
        selectedPipe = alienPipe[0].transform;
        enemyTransform = enemyStateMachine.transform;
        for (int i = 0; i < alienPipe.Count; i++)
        {
            if (Vector3.Distance(enemyStateMachine.transform.position, alienPipe[i].transform.position) < Vector3.Distance(enemyStateMachine.transform.position, selectedPipe.position))
            {
                selectedPipe = alienPipe[i].transform;
            }
        }
        enemyStateMachine.ChangeState(enemyStateMachine.ventingState);
    }

    public void UnventDirector(out Transform transform)
    {
        selectedPipe = alienPipe[0].transform;
        for (int i = 0; i < alienPipe.Count; i++)
        {
            if (Vector3.Distance(playerTransform.position, alienPipe[i].transform.position) < Vector3.Distance(playerTransform.position, selectedPipe.position))
            {
                selectedPipe = alienPipe[i].transform;
            }
        }
        transform = selectedPipe;
    }


    public void SpawnEnemyFirstTime()
    {
        enemyStateMachine.FirstSpawn();
    }
}