using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestingTimer : MonoBehaviour
{

    [SerializeField] float timeToRestMaster, timeToRest;
    [SerializeField] bool startedCor;
    [SerializeField] EnemySM stateMachine;
    [SerializeField] float waitTimer, timerIncrease, timerTotal;

    private void Awake()
    {
        stateMachine = FindObjectOfType<EnemySM>();
    }

    private void Update()
    {
        if (!startedCor && stateMachine.GetCurrentState().ToString() != "Resting")
        {
            StartCoroutine(RestTimer());
        }

        if (stateMachine.GetCurrentState().ToString() == "Idle")
        {
            waitTimer = 0.5f;
            timerIncrease = 1f;
            timeToRestMaster = 60f;
        }
        else if (stateMachine.GetCurrentState().ToString() == "Hunting")
        {
            waitTimer = 0.25f;
            timerIncrease = 2f;
            timeToRestMaster = 120f;
        }
    }


    IEnumerator RestTimer()
    {
        startedCor = true;
        while (stateMachine.GetCurrentState().ToString() != "Resting" && timeToRest < timeToRestMaster)
        {
            yield return new WaitForSeconds(waitTimer);
            if (stateMachine.GetCurrentState().ToString() == "Idle")
            {
                timeToRest += timerIncrease;
            }
            else if (stateMachine.GetCurrentState().ToString() != "Hunting")
            {
                timeToRest += timerIncrease;
            }
        }

        stateMachine.ChangeState(stateMachine.restingState);
        startedCor = false;
    }

    public void SetTimeToRest(float newState)
    {
        timeToRest = newState;
    }

}
