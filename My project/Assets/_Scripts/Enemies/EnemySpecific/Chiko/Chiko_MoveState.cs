using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Chiko_MoveState : MoveState
{
    private Chiko chiko;

    public Chiko_MoveState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, Chiko chiko) : base(etity, stateMachine, animBoolName, stateData)
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

        if (isPlayerInMinAgroRange)
        {
            stateMachine.ChangeState(chiko.playerDetectedState);
        }
        else if(isDetectingWall || !isDetectingLedge)
        {
            chiko.idleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(chiko.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
