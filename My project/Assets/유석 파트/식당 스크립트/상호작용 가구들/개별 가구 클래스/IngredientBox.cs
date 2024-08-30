using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class IngredientBox : RestaurantComponent, IInteractable
{
    public SamplePlayer player {get; private set;}
    public RecipeBook book;
    public RestaurantInventory boxInventory {get; set;}
    public bool isRestaurantOpened = true;

    public IngredientBoxInterface myInterface;

    protected override void Awake()
    {
        base.Awake();

        this.boxInventory = GetComponentInChildren<RestaurantInventory>();
    }

    public void Interact(GameObject interactRequester)
    {
        player = interactRequester.GetComponent<SamplePlayer>();

        // 납품하기
        if(!isRestaurantOpened)
        {
            // 여기에서 RPG 파트 인벤토리와 상호작용해서 RPG 파트의 인벤토리 아이템을 옮겨와야 함.


        }
        // 식재료 꺼내기
        else if(isRestaurantOpened)
        {
            Debug.Log("인터페이스 활성화!");
            myInterface.ActiveInterface(this);
        }
    }

    public bool IsShouldHidePlayer()
    {
        return false;
    }
}
