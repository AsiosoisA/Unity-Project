using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerObjectPool : ObjectPool
{
    public static CustomerObjectPool Instance;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }
}
