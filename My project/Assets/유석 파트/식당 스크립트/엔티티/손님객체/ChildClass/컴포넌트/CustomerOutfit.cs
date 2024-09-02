using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerOutfit : CustomerComponent
{
    protected override void Awake()
    {
        base.Awake();

        componentName = "outfit";
    }
}
