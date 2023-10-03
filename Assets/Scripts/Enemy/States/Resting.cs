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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
