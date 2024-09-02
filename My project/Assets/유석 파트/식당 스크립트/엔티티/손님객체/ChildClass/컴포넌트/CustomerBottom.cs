using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBottom : CustomerComponent
{
    protected override void Awake()
    {
        base.Awake();

        componentName = "bottom";
    }
}
