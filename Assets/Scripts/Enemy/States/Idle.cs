using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Idle : BaseState
{
    private EnemySM _SM;


    public Idle(EnemySM stateMachine) : base(stateMachine)
    {
        _SM = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        _SM.playerPos = Vector3.zero;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if(!_SM.heardPlayer)
        {
            Debug.Log("Hasn't heard");
            //Random movement
            _SM.timeToNextStep -= Time.deltaTime;
            if (_SM.timeToNextStep <= 0)
            {
                //play audiosource
                _SM.SpawnScanner();
                _SM.timeToNextStep = _SM.timeToNextStepMaster;
            }
        }
        else
        {
            Debug.Log("Heard player");
            _SM.ChangeState(_SM.huntingState);
        }
    }


    void RandomMovement()
    {

    }

    public override void Exit()
    {
        base.Exit();
        Transform playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 playerPos = new(playerTr.position.x, 1, playerTr.position.z);

        _SM.playerPos = playerPos;

    }

}
