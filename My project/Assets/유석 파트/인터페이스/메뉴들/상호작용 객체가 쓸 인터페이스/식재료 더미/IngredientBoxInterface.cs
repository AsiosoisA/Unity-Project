using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class IngredientBoxInterface : InterfaceMenu
{
    public IngredientBox box {get; private set;}
    
    public USObjectPool playerInventoryObjectPool;
    public USObjectPool playerRecipeObjectPool;

    
    private List<GameObject> playerInventoryPrefabs = new List<GameObject>();
    private List<GameObject> recipeListPrefabs = new List<GameObject>();

    protected override void Awake()
    {
        base.Awake();

        SetToggleKey(KeyCode.None);
        IgnoreActiveInput();
    }

    public virtual void ActiveInterface(IngredientBox box)
    {
        this.box = box;
        OnActive();
        Sync();
        //끌 때는 ESC 눌러서 끄면 된다!
    }

    public void Sync()
    {
        SyncPrefabCount();
        UpdatePlayerInventory();
        UpdateRecipeList();
    }

#region 동기화 메소드
    //플레이어의 인벤토리와 인터페이스의 인벤토리 상태를 동기화
    protected virtual void SyncPrefabCount()
    {
        if(playerInventoryPrefabs.Count != box.player.restaurantInventory.foodsICarrying.Keys.Count)
        {

            Debug.Log("인벤토리 칸과 프리팹 개수가 맞지 않습니다.");

            int repeatCount = 0;

            while(playerInventoryPrefabs.Count > box.player.restaurantInventory.foodsICarrying.Keys.Count)
            {
                playerInventoryObjectPool.Release(playerInventoryPrefabs[playerInventoryPrefabs.Count - 1]);
                playerInventoryPrefabs.RemoveAt(playerInventoryPrefabs.Count - 1);
                repeatCount++;

                if(repeatCount > 100)
                {
                    Debug.LogError("으악!!!!!!! 무한루프 발생!!!!");
                    break;
                } 
            }
            while(playerInventoryPrefabs.Count < box.player.restaurantInventory.foodsICarrying.Keys.Count)
            {
                playerInventoryPrefabs.Add(playerInventoryObjectPool.Get());
            
                repeatCount++;
                if(repeatCount > 100)
                {
                    Debug.LogError("으악!!!!!!! 무한루프 발생!!!!");
                    break;
                } 
            }
        }
        
        if(recipeListPrefabs.Count != box.player.restaurantInventory.holdingOrders.Count)
        {

            Debug.Log("레시피 칸과 프리팹 개수가 맞지 않습니다." + recipeListPrefabs.Count + " vs " + box.player.restaurantInventory.holdingOrders.Count);

            int repeatCount = 0;

            while(recipeListPrefabs.Count > box.player.restaurantInventory.holdingOrders.Count)
            {
                playerRecipeObjectPool.Release(recipeListPrefabs[recipeListPrefabs.Count - 1]);
                recipeListPrefabs.RemoveAt(recipeListPrefabs.Count - 1);

                repeatCount++;

                if(repeatCount > 100)
                {
                    Debug.LogError("으악!!!!!!! 무한루프 발생!!!!");
                    break;
                } 
            }
            while(recipeListPrefabs.Count < box.player.restaurantInventory.holdingOrders.Count)
            {
                recipeListPrefabs.Add(playerRecipeObjectPool.Get());

                repeatCount++;

                if(repeatCount > 100)
                {
                    Debug.LogError("으악!!!!!!! 무한루프 발생!!!!");
                    break;
                } 
            }    
        }
    }
    protected virtual void UpdatePlayerInventory()
    {
        Dictionary<string, FoodStuffAndCount> inventory = box.player.restaurantInventory.foodsICarrying;

        int index = 0;
        foreach(string key in inventory.Keys)
        {
            IngredientBoxPlayerItem item = playerInventoryPrefabs[index].GetComponent<IngredientBoxPlayerItem>();
            item.InitCell(inventory[key].foodStuff, inventory[key].count);

            PutInventoryItem(item, index++);
        }
    }
    protected void PutInventoryItem(IngredientBoxPlayerItem item, int index)
    {
        /*
            플레이어 인벤토리 배치 규칙

            %5 를 해서 나온 수만큼 + 100 을 PosX에 더한다.
            /5 를 해서 나온 수만큼 - 100 을 PosY에 더한다.
        */

        SetPosX(item.rectTransform, 50);
        SetPosY(item.rectTransform, -50);
        SetWidth(item.rectTransform, 100);
        SetHeight(item.rectTransform, 100);


        SetPosX(item.rectTransform, GetPosX(item.rectTransform) + (index % 5) * 100);
        SetPosY(item.rectTransform, GetPosY(item.rectTransform) + (index / 5) * (-100));
    }

    protected virtual void UpdateRecipeList()
    {
        List<Order> HoldingOrders = box.player.restaurantInventory.holdingOrders;

        int index = 0;
        foreach(Order order in HoldingOrders)
        {

            if(order.isPullIngredients)
            {
                // 이미 아이템을 뺀 레시피다! 얘는 표기할 필요가 없음.
                playerRecipeObjectPool.Release(recipeListPrefabs[recipeListPrefabs.Count - 1]);
                recipeListPrefabs.RemoveAt(recipeListPrefabs.Count - 1);           

                continue;
            }

            IngredientBoxRecipeItem item = recipeListPrefabs[index].GetComponent<IngredientBoxRecipeItem>();
            item.Init(this, order);
            PutRecipeItem(item, index++);
        }
    }
    protected void PutRecipeItem(IngredientBoxRecipeItem item, int index)
    {
        /*
            플레이어 레시피 배치 규칙

            그냥 index * (-110) 을 해버리면 끝.
        */
        SetLeft(item.rectTransform, 10);
        SetPosY(item.rectTransform, -60);
        SetRight(item.rectTransform, 10);
        SetHeight(item.rectTransform, 100);


        SetPosY(item.rectTransform, GetPosY(item.rectTransform) + index * (-110));
    }
#endregion
}