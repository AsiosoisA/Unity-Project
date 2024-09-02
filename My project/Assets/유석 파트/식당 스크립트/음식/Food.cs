using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Food : FoodStuff
{
    public abstract void unlockCheck();
    protected override void Awake()
    {
        base.Awake();

        // recipe 에 재료 추가 : 자식 클래스에서 할거임.

        // SO_Minigame 추가 : 직전 자식 클래스에서 할거임.

        // 그럼 얜 뭐하냐? 아무것도 안 한다!
    }
}
