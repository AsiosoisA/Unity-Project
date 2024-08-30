using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxMonsterMeat : Meat
{
    public static string instanceName = "여우몬 고기"; 

    protected override void Awake()
    {
        base.Awake();

        price = 10; // 한 10골드정도로 해야겠다

        foodStuffName = instanceName;
    }
}