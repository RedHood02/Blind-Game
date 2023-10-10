using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Hunting : BaseState
{
    private EnemySM _SM;

    public Hunting(EnemySM stateMachine) : base(stateMachine)
    {
        _SM = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        _SM.timerToReset = _SM.timerMaster;
        _SM.agent.SetDestination(_SM.playerPos);
        _SM.agent.speed = 7f;
        _SM.timeToNextStepMaster = 0.75f;
        _SM.meshRend.enabled = true;
        _SM.anim.Play("Spawn");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        LowerTimer();

    }

    void LowerTimer()
    {
        _SM.timerToReset -= Time.deltaTime;
        if (_SM.timerToReset <= 0)
        {
            _SM.ChangeState(_SM.idleState);
        }

        _SM.timeToNextStep -= Time.deltaTime;
        if (_SM.timeToNextStep <= 0)
        {
            _SM.source.GenerateImpulse();
            _SM.emitter.Play();
            _SM.SpawnScanner(1); //Running Scanner
            _SM.timeToNextStep = _SM.timeToNextStepMaster;
        }
    }

    public override void Exit()
    {
        base.Exit();
        _SM.heardPlayer = false;
        _SM.anim.Play("Despawn");
    }

    public void ResetTimer()
    {
        _SM.timerToReset = _SM.timerMaster;

        Transform playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 playerPos = new(playerTr.position.x, 1, playerTr.position.z);

        _SM.playerPos = playerPos;
        _SM.agent.SetDestination(_SM.playerPos);
    }



}
