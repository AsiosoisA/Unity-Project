using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxMonsterSushi : Sushi
{
    public static string instanceName = "여우몬 초밥";

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
