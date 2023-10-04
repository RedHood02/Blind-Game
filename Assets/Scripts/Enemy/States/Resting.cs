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
        int rand = (int)Random.Range(0f, _SM.restingSpotsList.Count);

        _SM.enemyLight.enabled = false;
        _SM.agent.SetDestination(_SM.restingSpotsList[rand].position);
    }

	public override void UpdateLogic()
	{
        Debug.Log("Started");
		base.UpdateLogic();

        if(_SM.agent.remainingDistance < 0.2f)
        {
            _SM.agent.isStopped = true;
        }

        if(_SM.agent.isStopped == true)
        {
            _SM.StartCoroutine(_SM.ResetMovement());
        }
	}

    public override void Exit()
    {
        base.Exit();
        _SM.enemyLight.enabled = true;
        _SM.agent.isStopped = false;
        _SM.ResetTimer();
    }
}
