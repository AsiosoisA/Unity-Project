using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public abstract class RestaurantComponent : StructureComponent
{
    protected RestaurantComponents componentContainer;
    protected Restaurant restaurant;
    protected bool isUsing = false;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetContainer(RestaurantComponents container){
        this.componentContainer = container;
    }

    protected virtual void UseThis() => isUsing = true;
    public virtual void UseThisToThisRestaurant(Restaurant restaurant)
    {
        UseThis();
        this.restaurant = restaurant;
    }
    protected virtual void NoLongerUseThis() => isUsing = false;

    protected override void LogicUpdate(){
        base.LogicUpdate();

        //TODO 최적화 가능

        if(isUsing) transform.gameObject.SetActive(true);
        else transform.gameObject.SetActive(false);


    }
}
