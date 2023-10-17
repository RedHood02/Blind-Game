using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    public static Director _Director;

    public List<GameObject> checkpoints = new(), alienPipe = new(), waterPipe = new();

    public EnemySM enemyStateMachine;

    private void Awake()
    {
        _Director = this;
    }



    public void SpawnEnemyFirstTime()
    {
        enemyStateMachine.FirstSpawn();
    }
}