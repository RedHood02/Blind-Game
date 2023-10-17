using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Resting : BaseState
{
    private EnemySM _SM;
    public Resting(EnemySM stateMachine) : base(stateMachine)
    {
        _SM = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        _SM.agent.isStopped = true;
        _SM.enemyLight.enabled = false;
    }


    public override void Exit()
    {
        base.Exit();
        _SM.agent.isStopped = false;
        _SM.enemyLight.enabled = true;
    }
}
