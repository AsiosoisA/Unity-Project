using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RestaurantComponent : StructureComponent
{
    protected RestaurantComponents componentContainer;

    protected override void Awake()
    {
        base.Awake();

        componentContainer = transform.parent.GetComponent<RestaurantComponents>();
    }
}
