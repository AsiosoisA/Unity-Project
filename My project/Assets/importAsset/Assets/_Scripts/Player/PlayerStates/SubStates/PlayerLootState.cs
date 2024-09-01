using Bardent.CoreSystem;
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
                // Ű�� ó�� ������ holdStartTime�� �����ϰ�, isHolding�� true�� ����
                holdStartTime = Time.time;
                isHolding = true;
            }
            else if (!isLooted)
            {
                // Ű�� ������ �ִ� ���� ����� �ð��� holdingTime�� ������ �׼��� ����
                if (Time.time >= holdStartTime + holdingTime)
                {
                    Loot();
                    isHolding = false; // �׼� ���� �� �ٽ� �ʱ�ȭ
                    stateMachine.ChangeState(player.IdleState);
                    isLooted = true;
                }
            }
        }
        else
        {
            // Ű�� ������ ���¸� �ʱ�ȭ�ϰ� GroundedState�� ��ȯ
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

        return closestEnemy; // ���� ����� "Enemy" �±׿� "Dead" ���̾ ���� ������Ʈ ��ȯ
    }

    private void Loot()
    {
        // �̴ϰ��� �Ŵ���? 


        Debug.Log(lootingObject.name);
    }
    #endregion
}