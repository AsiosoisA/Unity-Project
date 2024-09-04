using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBase : CustomerComponent
{
    protected override void Awake()
    {
        base.Awake();

        componentName = "base";
    }

    public void SitDownFinishTrigger()
    {
        this.MyCustomer.SitDownFinishTrigger();
    }

    public void StandUpFinishTrigger()
    {
        this.MyCustomer.StandUpFinishTrigger();
    }
}
