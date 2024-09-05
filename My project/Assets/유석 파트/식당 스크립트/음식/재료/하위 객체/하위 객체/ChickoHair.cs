using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickoHair : PlantBaseIngredient
{
    public static string instanceName = "칙코의 볏"; 

    protected override void Awake()
    {
        base.Awake();

        price = 0; // 한 10골드정도로 해야겠다

        foodStuffName = instanceName;
    }
}
