using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Minigame Custom Data", menuName = "Scriptable Objects/Minigame/KeyMinigame/HoldKeyMinigame")]
public class SO_HoldKeyMinigame : SO_KeyMinigame
{
    [Header("게이지에 대한 Y축 오프셋.")]
    public float extraOffset;
    [Header("눌렀을 때 게이지가 올라가는 속도.")]
    public float increaseSpeed;

    [Header("버튼을 안 눌렀을 때 게이지가 내려갈지 여부.\n 체크 : 게이지가 내려갑니다.")]
    public bool isGuageDecreaseWithoutPush;

    [Header("게이지가 내려간다면 즉시 삭제될 것인지 천천히 떨어질 것인지 결정합니다.\n체크 : 게이지가 즉시 사라집니다.")]
    public bool isImmediatelyDecrease;

    [Header("안 눌렀을 때 게이지가 줄어드는 속도.")]
    public float decreaseSpeed;



    [Header("아니면 안 누르면 그냥 게임 오버")]
    public bool isMinigameOverWhenNotPush;

    


    [Header("각각의 키에 대한 동작을 결정하는 SO 클래스입니다.")]
    public SO_KeyCustom keyCustom;

    public override Type GetMinigameType()
    {
        return typeof(HoldKeyMinigame);
    }
}
