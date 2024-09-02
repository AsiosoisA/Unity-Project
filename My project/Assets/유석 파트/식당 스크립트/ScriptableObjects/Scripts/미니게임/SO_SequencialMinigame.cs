using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Minigame Custom Data", menuName = "Scriptable Objects/Minigame/KeyMinigame/SequencialKeyMinigame")]
public class SO_SequencialKeyMinigame : SO_KeyMinigame
{
    [Header("등장할 키의 총 개수를 의미합니다.")]
    public int keyCount = 0; // 입력해야 하는 총 키의 갯수를 여기에 출력한다.
    [Header("keySequence 에 있는 키가 랜덤으로 등장합니다.")]
    public bool isRandom = false;
    [Header("keySequence 에 있는 키가 입력한 순서 그대로 등장합니다.")]
    public bool isRegular = false;

    [Header("각각의 키에 대한 동작을 결정하는 SO 클래스입니다.")]
    public SO_KeyCustom keyCustom;

    public override Type GetMinigameType()
    {
        return typeof(SequencialKeyMinigame);
    }
}