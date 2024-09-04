using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rice : PlantBaseIngredient
{
    public static string instanceName = "쌀"; 

    protected override void Awake()
    {
        base.Awake();

        price = 0; // 한 10골드정도로 해야겠다

        foodStuffName = instanceName;
    }
}
