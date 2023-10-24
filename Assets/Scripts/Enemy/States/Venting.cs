using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Venting : BaseState
{
	private EnemySM _SM;

	public Venting(EnemySM stateMachine) : base(stateMachine)
	{
		_SM = stateMachine;
	}

	public override void Enter()
	{
		base.Enter();
		GetNearestPipe();
		_SM.agent.SetDestination(_SM.selectedPipe.position);
		_SM.ventingTimer = _SM.ventingTimerMaster / 2;
		_SM.ventEmitter.SetParameter("Venting", 0);
	}

	public override void UpdateLogic()
	{
		base.UpdateLogic();
		_SM.timeToNextStep -= Time.deltaTime;
		if (!_SM.isVented)
		{
			Scanner();
		}
		if (_SM.inArea)
		{
			_SM.isVented = true;
			_SM.meshRend.enabled = false;
			_SM.enemyLight.enabled = false;
		}

		VentedTimer();
	}

	void VentedTimer()
	{
		if (_SM.isVented)
		{
			_SM.ventingTimer -= Time.deltaTime;
			if (_SM.ventingTimer < 0)
			{
				_SM.ChangeState(_SM.idleState);
			}
		}
	}

	void GetNearestPipe()
	{
		_SM.selectedPipe = _SM.alienPipes[0];
		for (int i = 0; i < _SM.alienPipes.Count; i++)
		{
			if (Vector3.Distance(_SM.transform.position, _SM.alienPipes[i].position) < Vector3.Distance(_SM.transform.position, _SM.selectedPipe.position))
			{
				_SM.selectedPipe = _SM.alienPipes[i];
			}
		}
	}

	void Scanner()
	{
		if (_SM.timeToNextStep <= 0)
		{
			_SM.source.GenerateImpulse();
			_SM.emitter.Play();
			_SM.SpawnScanner(0); // Walking scanner
			_SM.timeToNextStep = _SM.timeToNextStepMaster;
		}
	}

	void Unvent()
	{
		_SM.selectedPipe = _SM.alienPipes[0];
		for (int i = 0; i < _SM.alienPipes.Count; i++)
		{
			if (Vector3.Distance(_SM.playerPos, _SM.alienPipes[i].position) < Vector3.Distance(_SM.playerPos, _SM.selectedPipe.position))
			{
				_SM.selectedPipe = _SM.alienPipes[i];
			}
		}
		_SM.isVented = false;
	}

	public override void Exit()
	{
		base.Exit();
		Unvent();
		_SM.transform.SetPositionAndRotation(_SM.selectedPipe.position, Quaternion.identity);
		_SM.meshRend.enabled = true;
		_SM.enemyLight.enabled = true;
		_SM.isVented = false;
		_SM.ventEmitter.SetParameter("Venting", 1);
		_SM.ventEmitter.Play();
	}
}
