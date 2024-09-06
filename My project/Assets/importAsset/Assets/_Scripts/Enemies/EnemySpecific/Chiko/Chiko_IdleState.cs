using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chiko_IdleState : IdleState
{
    private Chiko chiko;

    public Chiko_IdleState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, Chiko chiko) : base(etity, stateMachine, animBoolName, stateData)
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
        Debug.Log("Chiko_Idle");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isPlayerInMinAgroRange)
        {
            stateMachine.ChangeState(chiko.playerDetectedState);
        }
        if (isIdleTimeOver)
        {
            stateMachine.ChangeState(chiko.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
