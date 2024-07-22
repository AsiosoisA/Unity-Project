using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PigRiceSoup : RiceSoup
{
    void Start(){
        foodName = "돼지국밥";
    }

    public override void unlockCheck()
    {
        // 언락 조건 확인해주면 됨.
    }
}
