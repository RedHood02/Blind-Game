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
        int rand = Random.Range(0, _SM.restingSpotsList.Count + 1);
        
        _SM.agent.SetDestination(_SM.restingSpotsList[rand].position);
    }

	public override void UpdateLogic()
	{
		base.UpdateLogic();
	}
}
