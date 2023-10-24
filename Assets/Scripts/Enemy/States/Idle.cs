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
        _SM.ventingTimer = 0;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        _SM.RandomiseSound();
        if (!_SM.heardPlayer)
        {
            if (_SM.agent.remainingDistance < 0.2f)
            {
                _SM.RandomPoint(_SM.transform.position, _SM.radius, out Vector3 point);
                _SM.agent.SetDestination(point);
            }
            Scanner();

            IncreaseVentTimer();
        }
        else
        {
            _SM.ChangeState(_SM.huntingState);
        }
    }

    void IncreaseVentTimer()
    {
        _SM.ventingTimer += Time.deltaTime;

        if(_SM.ventingTimer > _SM.ventingTimerMaster)
        {
            _SM.ChangeState(_SM.ventingState);
        }
    }
    void Scanner()
    {
        _SM.timeToNextStep -= Time.deltaTime;
        if (_SM.timeToNextStep <= 0)
        {
            _SM.source.GenerateImpulse();
            _SM.emitter.Play();
            _SM.SpawnScanner(0); // Walking scanner
            _SM.timeToNextStep = _SM.timeToNextStepMaster;
        }
    }

}
