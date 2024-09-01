using Bardent.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class PlayerState
{
    #region Componenets
    protected Movement Movement
    {
        get => movement ?? core.GetCoreComponent(ref movement);
    }
    private Movement movement;

    protected Core core;

    protected Player player;
    protected CameraHandler cameraHandler;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;
    #endregion

    #region Variables
    protected bool isAnimationFinished;
    protected bool isExitingState;
    protected bool isWindDashing { get; set; }

    protected float startTime;

    private string animBoolName;

    private float originalTimeScale = 1f;
    private float dashTimeScale = 0.5f;  // 환경의 속도를 50%로 늦춤
    private float dashDuration = 2f;     // 대쉬 지속 시간
    private float dashSpeedMultiplier = 2f; // 플레이어의 속도를 두 배로 빠르게
    private float animationSpeedMultiplier = 2f;
    private float originalAnimationSpeed = 1f;  // 원래 애니메이션 속도
    private float originalGravityScale;
    #endregion

    #region Unity Callback Functions
    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
        core = player.Core;
    }

    public virtual void Enter()
    {
        DoChecks();
        player.Anim.SetBool(animBoolName, true);
        startTime = Time.time;
        //Debug.Log(animBoolName);
        isAnimationFinished = false;
        isExitingState = false;
        originalGravityScale = player.RB.gravityScale;

        cameraHandler = GameObject.Find("Camera Handler").GetComponent<CameraHandler>();
    }

    public virtual void Exit()
    {
        player.Anim.SetBool(animBoolName, false);
        player.RB.gravityScale = originalGravityScale;
        isExitingState = true;
    }

    public virtual void LogicUpdate()
    {
        if (!cameraHandler.CheckIfInitZoom() && player.SuperJumpState.canChangeState)
        {
            cameraHandler.ResetCameraZoom();
        }

        if (player.InputHandler.Skill2Input && !isWindDashing)
        {
            isWindDashing = true;
            Time.timeScale = dashTimeScale; // 환경의 시간 느리게
            Time.fixedDeltaTime = 0.02f * Time.timeScale; // fixedDeltaTime 조정
            player.RB.gravityScale = originalGravityScale / dashTimeScale;

            Movement?.SetVelocity(Movement.CurrentVelocity.magnitude * dashSpeedMultiplier, Movement.CurrentVelocity.normalized);

            // 플레이어 애니메이터의 업데이트 모드를 Unscaled Time으로 변경
            player.Anim.updateMode = AnimatorUpdateMode.UnscaledTime;

            // 플레이어 동작에 슬로우모션 영향을 받지 않도록 별도의 타이머 사용
            player.StartCoroutine(ResetTimeAndSpeedAfterDuration(dashDuration));
        }
    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks() { }
    #endregion

    #region Animation Functions
    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
    #endregion

    #region Other Functions
    private IEnumerator ResetTimeAndSpeedAfterDuration(float duration)
    {
        yield return new WaitForSecondsRealtime(duration); // 실시간으로 대쉬 지속 시간 대기

        Time.timeScale = originalTimeScale; // 원래 시간 속도로 복구
        Time.fixedDeltaTime = 0.02f; // fixedDeltaTime 원래대로 복구

        // 애니메이터의 업데이트 모드를 Normal로 복구
        player.Anim.updateMode = AnimatorUpdateMode.Normal;
        player.RB.gravityScale = originalGravityScale;

        // 대쉬가 끝나면 플레이어의 속도를 원래대로 복구
        Movement?.SetVelocity(Movement.CurrentVelocity.magnitude / dashSpeedMultiplier, Movement.CurrentVelocity.normalized);
        isWindDashing = false;
    }
    #endregion
}