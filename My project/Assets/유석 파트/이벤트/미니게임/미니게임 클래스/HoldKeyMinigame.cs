using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldKeyMinigame : KeyMinigame
{
    public new SO_HoldKeyMinigame minigameData;
    public KeyInstance keyInstance;

    public HoldKeyMinigame(MinigameManager manager, SO_HoldKeyMinigame minigameData) : base(manager)
    {
        this.minigameData = minigameData;
        this.keyCustom = minigameData.keyCustom;
        
        keyInstance = manager.pool.Get(keyContainer.gameObject).GetComponent<KeyInstance>();

        /*
            우선 어떤 키코드를 사용하려 하는건지 알아내자.
        */
        string keyCode = minigameData.keyList[0];

        /*
            가져온 인스턴스에 대해 키코드를 먼저 초기화하자.
        */
        keyInstance.Init(keyCode, minigameData.keyCustom);

        /*
            키코드에 맞게 키의 이미지를 변경한다. (Sprite Library 와 Resolver를 이용한다!)
        */
        InitKeyCodeOfKey(keyInstance, keyCode);

        /*
            keyCustom 정보를 바탕으로 효과들을 할당한다.
        */
        keyInstance.custom = keyCustom;

        // 일단 숨길거다!
        keyInstance.gameObject.SetActive(false);

        manager.keyContainer.PlaceJustOneItem(keyInstance);
    }

    public override SO_MinigameData GetData() => minigameData;

    public override void StartMinigame(GameObject requester, Vector3 offset)
    {
        base.StartMinigame(requester, offset);

        keyInstance.gameObject.SetActive(true);

        keyContainer.SetPosition(requester, offset);

        keyContainer.guageBar.Init(keyContainer.transform, minigameData.extraOffset); // TODO 나중에 SO에 추가할 것.

        keyInstance.StartSensoring(this);

        isMinigaming = true;
    }

    public override void OnKeyPressed()
    {
        base.OnKeyPressed();

        keyContainer.guageBar.IncreaseGuage(minigameData.increaseSpeed); // 데이터에 추가할 것.

        if(keyContainer.guageBar.guageBar.value >= 0.99)
        {
            OnMinigameFinished();
        } 
    }

    public override void OnKeyUped()
    {
        base.OnKeyUped();
        
        if(minigameData.isGuageDecreaseWithoutPush)
        {
            if(minigameData.isImmediatelyDecrease) keyContainer.guageBar.DecreaseImmediately();
            else keyContainer.guageBar.DecreaseGuage(minigameData.decreaseSpeed);
        }
        else
        {
            keyContainer.guageBar.StopGuage();
        }
    }

    public override void OnMinigameFinished()
    {
        base.OnMinigameFinished();

        manager.pool.Release(keyInstance.gameObject);

        manager.keyContainer.RestorePosition();

        manager.keyContainer.guageBar.Release();

        manager.OnMinigameFinished();
    }
}
