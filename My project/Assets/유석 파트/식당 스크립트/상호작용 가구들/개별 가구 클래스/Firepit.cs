using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firepit : CookableRestaurantComponent
{
    public enum HowToCook
    {
        Bake,
        Roast
    }

    public SO_HoldKeyMinigame bakeMinigameData;
    public SO_HoldKeyMinigame roastMinigameData;

    private Animator animator;

    private bool isIdle;
    private bool isCookingStew;

    protected override void Awake()
    {
        base.Awake();

        structureName = "firepit";

        animator = GetComponent<Animator>();

        isIdle = true;
        isCookingStew = false;
    }

    protected override void LogicUpdate()
    {
        base.LogicUpdate();

        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isCookingStew", isCookingStew);
    }

    public override SO_MinigameData GetMinigameAndSetSprites(int howToCook)
    {
        if(howToCook == (int)HowToCook.Bake) return bakeMinigameData;
        else if(howToCook == (int)HowToCook.Roast) return roastMinigameData;
        else
        {
            Debug.LogError("올바르지 않은 접근입니다!");
            return null;
        }
    }

    public override void RequestToStartMinigame()
    {
        isIdle = false;
        isCookingStew = true;

        base.RequestToStartMinigame();
    }

    public override void OnMyMinigameFinished(string minigameName)
    {
        isIdle = true;
        isCookingStew = false;

        base.OnMyMinigameFinished(minigameName);
    }

    public override void OnMyMinigameFailed(string minigameName)
    {
        //base.OnMyMinigameFailed(minigameName);
        // 이 코드는 다음 아이템으로 건너뛰는 것. 즉 이 녀석의 로직과 맞지 않다!
        // 걍 종료해버리는게 목표.

        OnEntireMinigameFinished();
    }

    public override void OnEntireMinigameFinished()
    {
        isIdle = true;
        isCookingStew = false;

        base.OnEntireMinigameFinished();
    }
}
