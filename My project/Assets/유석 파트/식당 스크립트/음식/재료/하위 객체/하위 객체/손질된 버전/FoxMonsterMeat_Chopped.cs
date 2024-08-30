using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxMonsterMeat_Chopped : FoxMonsterMeat
{
    public new static string instanceName = "채썰기한 여우몬 고기";

    protected override void Awake()
    {
        base.Awake();

        foodStuffName = instanceName;
    }
}
