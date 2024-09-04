using Bardent.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    #region Input / Collision Variables
    protected int xInput;
    protected int yInput;

    protected bool isTouchingCeiling;

    private bool jumpInput;
    private bool grabInput;
    private bool subActionInput;
    private bool subActionInputStop;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingLedge;
    private bool dashInput;
    private bool interActionInput;
    private bool isLootableObject;
    private bool skill1Input;
    private bool skill2Input;
    private bool isCarvingRequested = false;
    #endregion

    #region Core Componenets


    private CollisionSenses CollisionSenses
    {
        get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses);
    }
    private CollisionSenses collisionSenses;
    #endregion

    #region Unity Callback Functions
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData,
        string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
            isTouchingWall = CollisionSenses.WallFront;
            isTouchingLedge = CollisionSenses.LedgeHorizontal;
            isTouchingCeiling = CollisionSenses.Ceiling;
        }
    }

    public override void Enter()
    {
        base.Enter();

        isLootableObject = false;

        player.JumpState.ResetAmountOfJumpsLeft();
        player.DashState.ResetCanDash();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        jumpInput = player.InputHandler.JumpInput;
        grabInput = player.InputHandler.GrabInput;
        dashInput = player.InputHandler.DashInput;
        skill1Input = player.InputHandler.Skill1Input;
        skill2Input = player.InputHandler.Skill2Input;
        interActionInput = player.InputHandler.InteractionInput;
        subActionInput = player.InputHandler.SubActionInput;
        subActionInputStop = player.InputHandler.SubActionInputStop;
        isLootableObject = CollisionSenses.DeadBody;

        if (player.InputHandler.InteractionInput && !isCarvingRequested)
        {
            isCarvingRequested = true; // 입력 처리 중으로 설정
            stateMachine.ChangeState(player.CarveState); // CarveState로 상태 전환
        }
        // InteractionInput이 없을 때 플래그를 초기화하여 다시 상태 전환이 가능하도록 함
        if (!player.InputHandler.InteractionInput)
        {
            isCarvingRequested = false;
        }
        if (player.InputHandler.AttackInputs[(int)CombatInputs.primary] && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.PrimaryAttackState);
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondary] && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.SecondaryAttackState);
        }
        else if (skill1Input)
        {
            player.InputHandler.UseSkill1Input();
            stateMachine.ChangeState(player.WindKnockbackSkillState);
        }
        else if (!subActionInputStop && jumpInput && player.JumpState.CanJump() && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.SuperJumpState);
        }
        else if (jumpInput && player.JumpState.CanJump() && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (!isGrounded)
        {
            player.InAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.InAirState);
        }
        else if (isTouchingWall && grabInput && isTouchingLedge)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
        else if (dashInput && player.DashState.CheckIfCanDash() && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.DashState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    #endregion
}