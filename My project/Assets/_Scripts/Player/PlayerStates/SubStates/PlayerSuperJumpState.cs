using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerSuperJumpState : PlayerAbilityState
{
    #region Variables
    private int amountOfJumpsLeft;

    public bool canChangeState;

    private float superJumpStartTime;
    private float maxHoldTime = 5f; 
    private float holdThreshold = 2f; 
    private float superJumpMultiplier;
    private float holdTime;

    private float initialCameraSize;
    private float maxZoomIn;
    private float maxZoomInFactor = 0.8f;
    private float zoomInSpeed = 0.5f;
    private float zoomOutSpeed = 0.5f;
    #endregion

    #region Unity Callback Functions
    public PlayerSuperJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        amountOfJumpsLeft = 0;
        isAbilityDone = false;
        Debug.Log("SJ State Entered");
        Movement?.SetVelocityZero();
        superJumpStartTime = Time.time;
        superJumpMultiplier = 1f;
        holdTime = 0f;

        maxZoomIn = initialCameraSize * maxZoomInFactor;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        holdTime = Time.time - superJumpStartTime;

        // 동시에 두 키를 누르고 있음
        if (!player.InputHandler.JumpInputStop && !player.InputHandler.SubActionInputStop)
        {
            canChangeState = false;
            cameraHandler.ApplyCameraZoom(holdTime, maxHoldTime, holdThreshold);
        }
        // Jump키를 놓았을 경우 
        else if (player.InputHandler.JumpInputStop)
        {
            canChangeState = true;
            // 임계치보다 짧게 눌렀다면 GroundedState로 전환
            if (holdTime < holdThreshold)
            { 
                stateMachine.ChangeState(player.IdleState);
                
            }
            // 임계치를 넘었을 경우 Super Jump 실행
            else
            { 
                float chargeTime = Mathf.Min(holdTime - holdThreshold, maxHoldTime - holdThreshold);
                superJumpMultiplier = 1f + (chargeTime / (maxHoldTime - holdThreshold));
                    
                ExecuteSuperJump();
                isAbilityDone = true;
                holdTime = 0f;
                stateMachine.ChangeState(player.InAirState); // 점프 후 GroundedState로 전환

            }
        } 
    }
    #endregion

    #region Other Functions
    private void ExecuteSuperJump()
    {
        // 슈퍼 점프의 최종 높이를 계산합니다.
        float superJumpHeight = playerData.jumpVelocity * superJumpMultiplier;
        cameraHandler.ResetFollowingObject();
        // 위쪽으로 Impulse 모드의 힘을 가합니다.
        Vector2 jumpForce = new Vector2(0, Mathf.Sqrt(2 * superJumpHeight * Physics2D.gravity.magnitude));
        player.RB.AddForce(jumpForce, ForceMode2D.Impulse);
    }
    #endregion
}
