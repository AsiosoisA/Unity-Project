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

        if (!player.InputHandler.JumpInputStop && !player.InputHandler.SubActionInputStop)
        {
            canChangeState = false;
            //TODO Zoom Not Wokring
            cameraHandler.ApplyCameraZoom(holdTime, maxHoldTime, holdThreshold);
        }
        else if (player.InputHandler.JumpInputStop)
        {
            canChangeState = true;
            if (holdTime < holdThreshold)
            {
                stateMachine.ChangeState(player.IdleState);

            }
            else
            {
                float chargeTime = Mathf.Min(holdTime - holdThreshold, maxHoldTime - holdThreshold);
                superJumpMultiplier = 1f + (chargeTime / (maxHoldTime - holdThreshold));

                ExecuteSuperJump();
                isAbilityDone = true;
                holdTime = 0f;
                stateMachine.ChangeState(player.InAirState);

            }
        }
    }
    #endregion

    #region Other Functions
    private void ExecuteSuperJump()
    {
        float superJumpHeight = playerData.jumpVelocity * superJumpMultiplier;
        cameraHandler.ResetFollowingObject();
        Vector2 jumpForce = new Vector2(0, Mathf.Sqrt(2 * superJumpHeight * Physics2D.gravity.magnitude));
        player.RB.AddForce(jumpForce, ForceMode2D.Impulse);
    }
    #endregion
}