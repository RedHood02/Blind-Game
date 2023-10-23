using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FMODUnity;
public class Venting : BaseState
{
    private EnemySM _SM;

    [SerializeField] StudioEventEmitter emitter;

    [SerializeField] float timer2 = 0;
    public Venting(EnemySM stateMachine) : base(stateMachine)
    {
        _SM = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        _SM.enemyLight.enabled = false;
        _SM.ventingEmitter.SetParameter("Venting", 0);
        Director._Director.Vent();
        _SM.agent.SetDestination(Director._Director.selectedPipe.position);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if(_SM.inArea)
        {
            _SM.Vent();
        }

        if(_SM.isVented)
        {
            float timer = Random.Range(90, 121);

            timer2 += Time.deltaTime;
            if(timer2 > timer)
            {
                _SM.ventingEmitter.SetParameter("Venting", 1);
                Director._Director.UnventDirector(out Transform pipe);
                _SM.Unvent(pipe);
                _SM.ChangeState(_SM.idleState);
            }
        }
    }


    public override void Exit()
    {
        base.Exit();
        timer2 = 0;
        _SM.enemyLight.enabled = true;
        _SM.inArea = false;
        _SM.isVented = false;
    }
}
