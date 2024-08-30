using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerHairstyle : CustomerComponent
{
    protected override void Awake()
    {
        base.Awake();

        componentName = "hairstyle";
    }
}
