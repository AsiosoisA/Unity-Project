using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class Table : RestaurantComponent, IInteractableStructure
{
    #region 테이블 정보
    public int tableId;
    public bool isAvailable = true; // 손님이 앉으면 false로 바뀜.
    
    [Header("0 : 왼쪽\n1 : 오른쪽\n2 : 정면\n3 : 뒤")]
    public int tableChairLookDirection;
    public const int LEFT = 0;
    public const int RIGHT = 1;
    public const int FRONT = 2;
    public const int BACK = 3;


    public float customerPositionOffset;
    #endregion

    #region 객체들
    private Customer customer; // 테이블에 현재 앉아 있는 손님   
    #endregion

    protected override void Awake(){
        base.Awake();
    }

    public void SetOwner(Customer customer)
    {
        isAvailable = false;
        customer.SetMyTable(this);
        this.customer = customer;
    }
    public void UnSetOwner(){
        customer.SetMyTable(null);
        customer = null;
        restaurant.PlusAvailableTableCount();
        isAvailable = true;
    }

    public void Interact(PlayerInteractState state, Player requester)
    {
        if(customer == null) return;

        string foodKey = customer.food_I_Want.foodStuffName;

        if(requester.restaurantInventory.foodsICarrying.ContainsKey(foodKey))
        {
            GiveThisFood(customer.food_I_Want); // TODO 플레이어가 들고 있는 음식을 주도록 해야 한다!
            requester.restaurantInventory.SubFoodFromPlayer(foodKey, 1);
        }
    }

    public bool IsShouldHidePlayer()
    {
        return false;
    }

    // 테이블에 음식을 전달하면 그건 그대로 손님에게 전달된다.
    public void GiveThisFood(Food food){
        if(customer != null) customer.GiveThisFood(food);
    }
}
