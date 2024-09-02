using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DeliveryToBoxInterface : InterfaceMenu
{
    public IngredientBox box {get; private set;}

    public USObjectPool playerInventoryObjectPool;
    
    private List<GameObject> boxInventoryPrefabs = new List<GameObject>();

    public Button deliveryButton;

    protected override void Awake()
    {
        base.Awake();

        SetToggleKey(KeyCode.None);
        IgnoreActiveInput();

        //deliveryButton.onClick.AddListener(DeliveryFoods); 직접 할당하세용;;
    }

    public virtual void ActiveInterface(IngredientBox box)
    {
        this.box = box;
        OnActive();
        Sync();
    }

    public void Sync()
    {
        SyncPrefabCount();
        UpdateBoxInventory();
    }

    public void DeliveryFoods()
    {
        Debug.Log("여기에서 플레이어의 인벤토리 내의 FoodStuff 태그 달린 녀석들이 전부 납품돼야 합니다!");

        bool isntFood = true;
        /*

            foreach( Item item in Sinyeong'sInventory)
            {
                FoodStuff food = item as FoodStuff
                if(food == null) continue;

                isntFood = false; // 납품할 식재료가 존재하니 false 로 변경.
                box.boxInventory.AddFoodToPlayer(food, 1);
                Remove Item from Sinyeong's Inventory;
            }
        */

        Sync();

        string popupContent;

        if(isntFood) popupContent = "납품할 식재료가 없습니다!";
        else popupContent = "식재료들을 모두 납품했습니다!";

        PopupManager.Instance.MakePopup(
            popupContent
        );
    }

#region 동기화 메소드

    protected virtual void SyncPrefabCount()
    {
        if(boxInventoryPrefabs.Count != box.boxInventory.foodsICarrying.Keys.Count)
        {
            Debug.Log("인벤토리 칸과 프리팹 개수가 맞지 않습니다!");
            int repeatCount = 0;

            while(boxInventoryPrefabs.Count > box.boxInventory.foodsICarrying.Keys.Count)
            {
                Debug.Log("그러므로 프리팹 개수를 줄이겠습니다!");
                playerInventoryObjectPool.Release(boxInventoryPrefabs[boxInventoryPrefabs.Count - 1]);
                boxInventoryPrefabs.RemoveAt(boxInventoryPrefabs.Count - 1);
                repeatCount++;

                if(repeatCount > 100)
                {
                    Debug.LogError("으악!!!!!!! 무한루프 발생!!!!");
                    break;
                } 
            }

            while(boxInventoryPrefabs.Count < box.boxInventory.foodsICarrying.Keys.Count)
            {
                Debug.Log("그러므로 프리팹 개수를 늘리겠습니다!");
                boxInventoryPrefabs.Add(playerInventoryObjectPool.Get());

                repeatCount++;

                if(repeatCount > 100)
                {
                    Debug.LogError("으악!!!!!!! 무한루프 발생!!!!");
                    break;
                } 
            }
        }
        else
        {
            Debug.Log("음. 완벽히 들어맞습니다.");
        }
    }

    protected virtual void UpdateBoxInventory()
    {
        Dictionary<string, FoodStuffAndCount> inventory = box.boxInventory.foodsICarrying;

        Debug.Log("엥?? 이거 발동함?");

        int index = 0;
        foreach(string key in inventory.Keys)
        {
            IngredientBoxPlayerItem item = boxInventoryPrefabs[index].GetComponent<IngredientBoxPlayerItem>();
            item.InitCell(inventory[key].foodStuff, inventory[key].count);

            PutInventoryItem(item, index++);
        }
    }

    protected void PutInventoryItem(IngredientBoxPlayerItem item, int index)
    {
        /*
            박스 인벤토리 규칙

            ㄴ 이거 아직 완성 안 했음.
        */

        SetPosX(item.rectTransform, 60);
        SetPosY(item.rectTransform, -60);
        SetWidth(item.rectTransform, 100);
        SetHeight(item.rectTransform, 100);

        SetPosX(item.rectTransform, GetPosX(item.rectTransform) + (index % 9) * 110);
        SetPosY(item.rectTransform, GetPosY(item.rectTransform) + (index / 9) * (-110));
    }

#endregion
}
