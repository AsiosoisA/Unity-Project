using Bardent.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class PlayerCarveState : PlayerGroundedState /* 유석 추가 부분 */ , IMinigamable
{
    #region Core Components
    private CollisionSenses CollisionSenses
    {
        get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses);
    }
    private CollisionSenses collisionSenses;
    #endregion

    #region Variables
    private GameObject carvingObject;
    private float holdStartTime;
    private bool interactionInput;
    private bool isHolding = false;
    private bool holdEnough = false;
    private bool isCarved = false;
    private float holdingTime = 2f;

    private int DeadLayer = LayerMask.NameToLayer("Dead"); // 레이어 정수화

    [SerializeField]
    private ItemHandler itemHandler;
    #endregion

    #region Unity Callback Functions
    public PlayerCarveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        carvingObject = GetClosestDeadEnemy(player.transform.position, playerData.deadBodyRadius);
        if(carvingObject == null) stateMachine.ChangeState(player.IdleState);
        else
        {
            holdStartTime = 0f;
            isHolding = false;   // 초기화
            isCarved = false;    // 초기화

            /*
                난입아트장인난입내용

                아
                자고싶다

                변경점
                Enter ( ) 에서 미니게임을 시작하고 종료를 기다림.

                미니게임 성공 / 실패 여부는 콜백 함수로 처리하며 LogicUpdate 는 발동하지 않음.
            */
            Debug.Log("카브스테이트 엔터");
            RequestToStartMinigame();
        }

    }

    public override void LogicUpdate()
    {
        Movement.SetVelocityZero();
        /*
        
        원래 있던 로직.

        base.LogicUpdate();

        interactionInput = player.InputHandler.InteractionInput;

        // 눌렀을 경우
        if (interactionInput)
        {
            // 처음 누르는 경우
            if (!isHolding)
            {
                holdStartTime = Time.time;
                isHolding = true;
            }

            // 일정 시간 이상 누르고 있을 경우 holdEnough 상태를 true로 설정
            if (!holdEnough && Time.time >= holdStartTime + holdingTime)
            {
                Carve();
                holdEnough = true;
                stateMachine.ChangeState(player.IdleState);
            }
        }
        // 키를 뗐을 경우
        else
        {
            if (isHolding && holdEnough)
            {
               
                isCarved = true;
            }
            stateMachine.ChangeState(player.IdleState);
            return; // 상태 전환 후 추가 로직이 실행되지 않도록 return
        }

        */
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
            if (collider.CompareTag("Enemy") && collider.gameObject.layer == DeadLayer) //임시로 8로 바꿈
            {
                float distance = Vector2.Distance(playerPosition, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = collider.gameObject;
                }
            }
        }

        return closestEnemy;
    }

    private void Carve()
    {
        //player.inventory.AddItem(carvingObject.GetComponent<Enemy1>().CavingItem);
        
        /*
        GetClosestDeadEnemy를 통해서 Layer가 Dead인 Enemy의 정보를 얻고 충분한 시간동안 interactionInput을 
        눌렀을 경우 Carve()를 실행하고 Idle로 탈출하도록 만들었습니다.

        신영님 편하신 대로 Enemy 정보에 따라 인벤토리에 아이템 추가하는 로직 작성하시면 될거 같아요!
        */
        Debug.Log("Carve");
        stateMachine.ChangeState(player.IdleState);
    }

    #endregion


    #region 유석 추가부분
    public GameObject GetGameObject()
    {
        return player.gameObject;
    }

    public Vector3 GetContainerOffset()
    {
        return new Vector3(0, 0.8f, 0);
    }

    public void RequestToStartMinigame()
    {
        player.minigameManager.CreateAndStartMinigame(this, player.carveMinigameData);
    }

    public void OnMyMinigameFinished(string minigameName)
    {
        Carve();
    }
    public void OnMyMinigameFailed(string minigameName)
    {
        stateMachine.ChangeState(player.IdleState);
    }

    public void OnKeyInputSuccessed(){} // Do Nothing! Just Implement.
    #endregion
}
