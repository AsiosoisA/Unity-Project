using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class IngredientBox : RestaurantComponent, IInteractableStructure
{
    public SamplePlayer player {get; private set;}
    public RecipeBook book;
    public RestaurantInventory boxInventory {get; set;}

    
    public IngredientBoxInterface getIngredientInterface;
    public DeliveryToBoxInterface putIngredientInterface;

    protected override void Awake()
    {
        base.Awake();

        this.boxInventory = GetComponentInChildren<RestaurantInventory>();
    }

    public void Interact(GameObject interactRequester)
    {
        player = interactRequester.GetComponent<SamplePlayer>();

        // 납품하기
        if(!restaurant.isOpened)
        {
            // 여기에서 RPG 파트 인벤토리와 상호작용해서 RPG 파트의 인벤토리 아이템을 옮겨와야 함.
            
            // 설계 : 일단 식당 인벤토리를 출력. 식재료 납품 버튼을 누르면 납품되는거까지만 만들자!
            
            putIngredientInterface.ActiveInterface(this);
        }
        // 식재료 꺼내기
        else
        {
            Debug.Log("인터페이스 활성화!");
            getIngredientInterface.ActiveInterface(this);
        }
    }

    public bool IsShouldHidePlayer()
    {
        return false;
    }
}
