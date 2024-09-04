using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantInventory : MonoBehaviour
{
    public Dictionary<string, FoodStuffAndCount> foodsICarrying = new Dictionary<string, FoodStuffAndCount>();
    public List<Order> holdingOrders = new List<Order>();
    public List<FoodStuff> debug_putFoodsBeforeGameStart = new List<FoodStuff>();
    public List<string> debug_currentFoods;
    public bool isDebugging;

    void Awake()
    {
        /* 디버그용 코드. FoodICarrying 리스트에 아이템을 넣어놓으면 플레이어는 게임 시작과 동시에 해당 음식들을 들고 있게 된다. */

        foreach(FoodStuff item in debug_putFoodsBeforeGameStart)
        {
            if(foodsICarrying.ContainsKey(item.foodStuffName)) foodsICarrying[item.foodStuffName].AddCount(1);
            else foodsICarrying.Add(item.foodStuffName, new FoodStuffAndCount(item, 1));
        }
    }

    public bool SubFoodFromPlayer(string foodName, int count)
    {
        bool isSuccess = false;

        foodsICarrying[foodName].SubCount(count);
        if(foodsICarrying[foodName].count <= 0)
        {
            isSuccess = true;
            foodsICarrying.Remove(foodName);
        }

        if(isDebugging) DebugListUpdate();
    
        return isSuccess;
    }

    public void AddFoodToPlayer(FoodStuff food, int count)
    {
        if(!foodsICarrying.ContainsKey(food.foodStuffName))
        {
            foodsICarrying.Add(food.foodStuffName, new FoodStuffAndCount(food, count));
        }
        else
        {
            foodsICarrying[food.foodStuffName].count += count;
        }

        if(isDebugging) DebugListUpdate();
    }

    public bool CanISubThis(List<FoodStuff> foods)
    {
        Dictionary<string, FoodStuffAndCount> paramList = new Dictionary<string, FoodStuffAndCount>();

        foreach(FoodStuff food in foods)
        {
            if(!paramList.ContainsKey(food.foodStuffName))
            {
                paramList.Add(food.foodStuffName, new FoodStuffAndCount(food, 1));
            }
            else
            {
                paramList[food.foodStuffName].count += 1;
            }
        }

        foreach(string key in paramList.Keys)
        {
            if(!foodsICarrying.ContainsKey(key))
            {
                return false;
            }
            if(paramList[key].count > foodsICarrying[key].count)
            {
                return false;
            }
        }

        return true;
    }

    public void RemoveMyOrder(Customer customer)
    {
        for(int i = 0; i < holdingOrders.Count; i++)
        {
            if(holdingOrders[i].orderer == customer)
            {
                holdingOrders.RemoveAt(i);
            }
        }
    }

    private void DebugListUpdate()
    {
        List<string> newlist = new List<string>();

        foreach(string key in foodsICarrying.Keys)
        {
            for(int i = 0; i < foodsICarrying[key].count; i++)
            {
                newlist.Add(key);
            }
        }

        debug_currentFoods = newlist;
    }
}
