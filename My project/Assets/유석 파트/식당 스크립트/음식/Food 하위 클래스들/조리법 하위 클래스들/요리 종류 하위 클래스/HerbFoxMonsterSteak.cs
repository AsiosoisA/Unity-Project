using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerbFoxMonsterSteak : Steak
{
    public static string instanceName = "허브를 곁들인 여우몬 스테이크";

    public override void unlockCheck()
    {
        Debug.Log("언락");
    }

    protected override void Awake()
    {
        base.Awake();

        foodStuffName = instanceName;
    }
}
