using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerTop : CustomerComponent
{
    protected override void Awake()
    {
        base.Awake();

        componentName = "top";
    }
}
