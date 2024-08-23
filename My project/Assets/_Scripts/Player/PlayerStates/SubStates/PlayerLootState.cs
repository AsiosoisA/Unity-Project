using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLootState : PlayerGroundedState
{
    #region Core Components
    private CollisionSenses CollisionSenses
    {
        get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses);
    }
    private CollisionSenses collisionSenses;
    #endregion

    #region Variables
    private GameObject lootingObject;
    private float holdStartTime;
    private bool interactionInput;
    private bool isHolding = false;
    private bool isLooted = false;
    private float holdingTime = 2f;

    [SerializeField]
    private ItemHandler itemHandler;
    #endregion

    #region Unity Callback Functions
    public PlayerLootState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Movement?.SetVelocityX(0f);

        lootingObject = GetClosestDeadEnemy(player.transform.position, playerData.deadBodyRadius);

        holdStartTime = 0f;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        interactionInput = player.InputHandler.InteractionInput;

        if (interactionInput)
        {
            if (!isHolding)
            {
                // 키를 처음 누르면 holdStartTime을 설정하고, isHolding을 true로 설정
                holdStartTime = Time.time;
                isHolding = true;
            }
            else if(!isLooted)
            {
                // 키를 누르고 있는 동안 경과한 시간이 holdingTime을 넘으면 액션을 수행
                if (Time.time >= holdStartTime + holdingTime)
                {
                    Loot();
                    isHolding = false; // 액션 수행 후 다시 초기화
                    stateMachine.ChangeState(player.IdleState);
                    isLooted = true;
                }
            }
        }
        else 
        {
            // 키를 놓으면 상태를 초기화하고 GroundedState로 전환
            if (isHolding && Time.time < holdStartTime + holdingTime)
            {
                stateMachine.ChangeState(player.IdleState);
            }

            isHolding = false;
        }
    }
    #endregion

    #region Other Functions
    public GameObject GetClosestDeadEnemy(Vector2 playerPosition, float checkRadius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(playerPosition, checkRadius, CollisionSenses.WhatIsLootable);

        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy") && collider.gameObject.layer == 10)
            {
                float distance = Vector2.Distance(playerPosition, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = collider.gameObject;
                }
            }
        }

        return closestEnemy; // 가장 가까운 "Enemy" 태그와 "Dead" 레이어를 가진 오브젝트 반환
    }

    private void Loot()
    {
        // 미니게임 매니저? 

        
        Debug.Log(lootingObject.name);
    }
    #endregion
}
