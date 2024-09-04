using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SO_KeyMinigame : SO_MinigameData
{
    [Header("키 배열을 집어넣는 리스트입니다. leftArrow, w, escape 등 키코드에 맞게 입력해주십시오.")]
    public List<string> keyList = new List<string>(); // leftArrow , rightArrow, upArrow, downArrow 등 미니게임에 사용할 키를 입력한다!
}
