using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chiko_StunState : StunState
{
    private Chiko chiko;

    public Chiko_StunState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData, Chiko chiko) : base(etity, stateMachine, animBoolName, stateData)
    {
        this.chiko = chiko;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
