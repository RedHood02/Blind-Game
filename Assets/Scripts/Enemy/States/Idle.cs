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
		_SM.agent.speed = 3.5f;
		_SM.timeToNextStepMaster = 5;
		_SM.playerPos = Vector3.zero;
		_SM.RandomPoint(_SM.transform.position, _SM.radius, out Vector3 point);
		_SM.agent.SetDestination(point);
	}

	public override void UpdateLogic()
	{
		base.UpdateLogic();
		if (!_SM.heardPlayer)
		{
			if (_SM.agent.remainingDistance < 0.2f)
			{
				_SM.RandomPoint(_SM.transform.position, _SM.radius, out Vector3 point);
				_SM.agent.SetDestination(point);
			}
			IncreaseTimer();
			Scanner();
		}
		else
		{
			_SM.ChangeState(_SM.huntingState);
		}
	}


	void IncreaseTimer()
	{
		_SM.IncreaseRestTimer(0.5f);
	}

	void Scanner()
	{
		_SM.timeToNextStep -= Time.deltaTime;
		if (_SM.timeToNextStep <= 0)
		{
			//play audiosource
			_SM.SpawnScanner(0); // Walking scanner
			_SM.timeToNextStep = _SM.timeToNextStepMaster;
		}
	}


	public override void Exit()
	{
		base.Exit();
		Transform playerTr = GameObject.FindGameObjectWithTag("Player").transform;
		Vector3 playerPos = new(playerTr.position.x, 1, playerTr.position.z);

		_SM.playerPos = playerPos;

	}

}
