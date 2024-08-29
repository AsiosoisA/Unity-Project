using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    private Animator anim; 

    private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }

    private Movement movement;
    private CollisionSenses collisionSenses;

    protected D_MoveState stateData;

    protected bool isDetectingWall;
    protected bool isDetectingLedge;
    protected bool isPlayerInMinAgroRange;

    private float speedMultiplier = 1f;  // 기본 속도 배수

    public MoveState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData) : base(etity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public void SetMovementSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isDetectingLedge = CollisionSenses.LedgeVertical;
        isDetectingWall = CollisionSenses.WallFront;
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();
        if(anim != null)
        {
            anim.speed = speedMultiplier;
        }
        Movement?.SetVelocityX(stateData.movementSpeed * speedMultiplier * Movement.FacingDirection);
    }

    public override void Exit()
    {
        base.Exit();
        if(anim != null)
        {
            anim.speed = 1f;
        }
        speedMultiplier = 1f;  // 상태를 떠날 때 속도 배수를 초기화
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement?.SetVelocityX(stateData.movementSpeed * speedMultiplier * Movement.FacingDirection);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

