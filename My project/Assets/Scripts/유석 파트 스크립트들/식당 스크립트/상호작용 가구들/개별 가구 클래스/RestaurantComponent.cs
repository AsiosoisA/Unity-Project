using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public abstract class RestaurantComponent : StructureComponent
{
    protected RestaurantComponents componentContainer;

    protected bool isUsing = false;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetContainer(RestaurantComponents container){
        this.componentContainer = container;
    }

    protected virtual void TakeThis() => isUsing = true;
    protected virtual void NoLongerUseThis() => isUsing = false;

    protected override void LogicUpdate(){
        base.LogicUpdate();

        if(isUsing) transform.gameObject.SetActive(true);
        else transform.gameObject.SetActive(false);


    }
}
