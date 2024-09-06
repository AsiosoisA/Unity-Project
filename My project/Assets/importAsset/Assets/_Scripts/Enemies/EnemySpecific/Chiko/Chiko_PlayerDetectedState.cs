using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chiko_PlayerDetectedState : PlayerDetectedState
{
    private Chiko chiko;

    public Chiko_PlayerDetectedState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetected stateData, Chiko chiko) : base(etity, stateMachine, animBoolName, stateData)
    {
        this.chiko = chiko;
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

        if ((performCloseRangeAction || performLongRangeAction) || !isDetectingLedge)
        {
            Movement?.Flip();

            // MoveState로 전환 시 속도를 두 배로 설정
            chiko.moveState.SetMovementSpeedMultiplier(1.5f);
            stateMachine.ChangeState(chiko.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

