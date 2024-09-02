using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Pool;

public class SequencialKeyMinigame : KeyMinigame
{
    public new SO_SequencialKeyMinigame minigameData;
    public List<KeyInstance> keyInstances = new List<KeyInstance>();



    /*
        각 키의 현재 상태를 추적하기 위한 변수들
    */
    private KeyInstance currentInstance;
    private int currentIndex;




    private bool isFirstAction = true;

    public SequencialKeyMinigame(MinigameManager manager, SO_SequencialKeyMinigame minigameData) : base(manager)
    {

        Debug.Log("새 미니게임을 만들었습니다! - 키 연속으로 누르기 미니게임!!! : " + minigameData.name);

        this.minigameData = minigameData;
        this.keyCustom = minigameData.keyCustom;

        //keyInstances 를 본격적으로 할당해보자.

        for(int i = 0; i < minigameData.keyCount; i++) // 할당하라고 한 키의 수만큼 반복
        {
            KeyInstance keyInstance = manager.pool.Get(keyContainer.gameObject).GetComponent<KeyInstance>();

            /*
                우선 어떤 키코드를 사용하려고 하는 건지를 알아내자.
            */
            string keyCode;
            if(minigameData.isRandom) keyCode = minigameData.keyList[Random.Range(0, minigameData.keyList.Count)];
            else keyCode = minigameData.keyList[i % minigameData.keyList.Count];

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

            keyInstances.Add(keyInstance);   
        }

        /*
            이제 컨테이너 안에서 키의 배치를 잘 배치하는 일만 남았음.
            TODO
        */
        manager.keyContainer.PlaceItems(keyInstances);
    }

    public override SO_MinigameData GetData() => minigameData;

    public override void StartMinigame(GameObject requester, Vector3 offset)
    {
        base.StartMinigame(requester, offset);
//
        keyContainer.SetPosition(requester, offset);

        // 얜 하나하나의 키가 완료됐는지를 추적해야 함.

        currentIndex = 0;
        currentInstance = keyInstances[currentIndex];
        isFirstAction = true;

        SetEffectOfContainerItems();

        currentInstance.StartSensoring(this);
    }

    public override void OnKeyInputSucessed()
    {
        manager.OnKeyInputSuccessed();

        currentIndex++; // 다음으로 관측할 녀석 결정.

        if(currentIndex != keyInstances.Count)
        {
            currentInstance = keyInstances[currentIndex];

            manager.keyContainer.MoveOneBlock();
            SetEffectOfContainerItems();

            // 센서링 스타트를 넣었으나 올바르게 동작하지 않음!
        }
        else
        {
            currentInstance = null;
        }
        // else game finished
    }

    public override void OnKeyLifecycleFinished()
    {
        if(currentIndex == keyInstances.Count)
        {
            foreach(KeyInstance keyInstance in keyInstances)
            {
                keyInstance.effectCore.ChangeScaleConst(keyCustom.keySize, 0f);
                manager.pool.Release(keyInstance.gameObject);
            }


            // 정상화해야지
            manager.keyContainer.RestorePosition();


            manager.OnMinigameFinished();
        }
        // 게임이 완료됐다!
    }

    public override void OnReadyToGetNextKey()
    {
        base.OnReadyToGetNextKey();

        if(currentInstance != null) currentInstance.StartSensoring(this);
    }

    public void SetEffectOfContainerItems()
    {
        if(keyInstances.Count <= currentIndex) Debug.LogError("모든 인덱스를 사용했는데도 SetEffectOfContainerItems 가 호출됐습니다!");

        //첫 번째 키
        SetActionForFirstItem(keyInstances[currentIndex]);
        //두 번째 키
        if(currentIndex + 1 < keyInstances.Count)
        SetActionForSecondItem(keyInstances[currentIndex + 1]);
        //세 번째 키
        if(currentIndex + 2 < keyInstances.Count)
        SetActionForSecondItem(keyInstances[currentIndex + 2]);
        //네 번째 키
        if(currentIndex + 3 < keyInstances.Count)
        SetActionForThirdItem(keyInstances[currentIndex + 3]);
        //나머지


        if(isFirstAction) isFirstAction = false;
    }


    private void SetActionForFirstItem(KeyInstance item)
    {
        if(isFirstAction)
        {
            item.gameObject.SetActive(true); // 이것만 하기
        }
        else
        {
            // 현재 첫 번째 요소로 선택된 녀석에게는 두 번째 효과가 적용되어 있음.
            // 근데 일단 SetActive만 하니까 괜찮을 듯.
        }
    }
    private void SetActionForSecondItem(KeyInstance item)
    {
        if(isFirstAction)
        {
            item.gameObject.SetActive(true);
            // 얘까지도 일단 이것만 적용하면 될 듯?
        }
        else
        {
            // 현재 두 번째 요소로 선택된 녀석에게는 세 번째 효과가 적용되어 있음.
            item.effectCore.ChangeScaleConst(keyCustom.keySize, 0.3f);
        }

    }
    private void SetActionForThirdItem(KeyInstance item)
    {
        if(true) // 어떤 순간이든 그냥 이걸 적용하면 됨. 기본적으로 얘가 적용된단건 기존에 SetActive(false) 인 놈들 뿐이니까.
        {
            item.effectCore.ChangeScaleConst(0.3f, 0f);
            item.gameObject.SetActive(true);
        }
    }
    private void SetActionForFourthItem(KeyInstance item)
    {
        if(true) // 어떤 순간이든 그냥 이걸 적용하면 됨. 기본적으로 얘가 적용된단건 기존에 SetActive(false) 인 놈들 뿐이니까.
        {
            item.effectCore.ChangeScaleConst(0.3f, 0f);
            item.gameObject.SetActive(true);
        }
    }
}
