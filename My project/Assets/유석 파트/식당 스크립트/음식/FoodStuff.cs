using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodStuff : MonoBehaviour
{
    // is 키워드로 Food 인지 Ingredient 인지 구분하자!
    public Sprite foodSprite;
    public string foodStuffName;
    public int price {get; protected set;}
    protected virtual void Awake(){}
}

[System.Serializable]
public class FoodStuffAndCount
{
    public FoodStuff foodStuff;
    public int count;

    public FoodStuffAndCount(FoodStuff foodStuff, int count)
    {
        this.foodStuff = foodStuff;
        this.count = count;
    }

    public void AddCount(int count)
    {
        this.count += count;
    }
    public void SubCount(int count)
    {
        this.count -= count;
    }
}
