using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxMonsterMeat_Sliced : FoxMonsterMeat
{
    public new static string instanceName = "슬라이스된 여우몬 고기";
    protected override void Awake()
    {
        base.Awake();

        foodStuffName = instanceName;
    }
}
