using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    public bool CanDash { get; private set; }

    private float lastDashTime;
    private Vector2 dashDirection;
    private Vector2 lastAIPos;

    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        CanDash = false;
        player.InputHandler.UseDashInput();

        dashDirection = Vector2.right * Movement.FacingDirection;

        Time.timeScale = 1f; // 시간 느려짐 효과 제거
        startTime = Time.time;

        player.RB.drag = playerData.drag;
        Movement?.SetVelocity(playerData.dashVelocity, dashDirection);
        PlaceAfterImage();
    }

    public override void Exit()
    {
        base.Exit();

        if (Movement?.CurrentVelocity.y > 0)
        {
            Movement?.SetVelocityY(Movement.CurrentVelocity.y * playerData.dashEndYMultiplier);
        }

        player.RB.drag = 0f; // 대시 종료 시 드래그 초기화
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            Movement?.SetVelocity(playerData.dashVelocity, dashDirection);
            CheckIfShouldPlaceAfterImage();

            if (Time.time >= startTime + playerData.dashTime)
            {
                isAbilityDone = true;
                lastDashTime = Time.time;
            }
        }
    }

    private void CheckIfShouldPlaceAfterImage()
    {
        if (Vector2.Distance(player.transform.position, lastAIPos) >= playerData.distBetweenAfterImages)
        {
            PlaceAfterImage();
        }
    }

    private void PlaceAfterImage()
    {
        PlayerAfterImagePool.Instance.GetFromPool();
        lastAIPos = player.transform.position;
    }

    public bool CheckIfCanDash()
    {
        return CanDash && Time.time >= lastDashTime + playerData.dashCooldown;
    }

    public void ResetCanDash() => CanDash = true;
}