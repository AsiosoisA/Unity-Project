using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class Table : RestaurantComponent
{
    #region 테이블 정보
    public int tableNumber;
    public bool isAvailable = true; // 손님이 앉으면 false로 바뀜.
    #endregion

    #region 객체들
    private Customer customer; // 테이블에 현재 앉아 있는 손님
    private Restaurant myRestaurant;
    public SpriteRenderer TableRenderer {get; private set;}
    
    #endregion

    protected override void Awake(){
        base.Awake();
        TableRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeThisTable(Restaurant restaurant){
        base.TakeThis();
        myRestaurant = restaurant;
    }

    public void SetCustomer(Customer customer){
        this.customer = customer;

        //TODO 고쳐야 하는 로직임.....!!!
        // 해법 : 레이어를 하나 더 만든다. Customer , Table
        // 테이블의 레이어가 무조건 Customer 보다 나중에 그려지게 만들어서 앉는 효과를 내야 한다.

        /*
            의자의 sorting layer 를 그대로 따른다.
            ㄴ 즉 이 부분은 삭제되어야 마땅함.
        */
        
        int orderInLayer = TableRenderer.sortingOrder;
        customer.spriteRenderer.sortingOrder = orderInLayer - 1; // 의자보다는 먼저 그려져야 함.
    }

    public void GetUp(){
        customer = null;
        myRestaurant.PlusAvailableTableCount();
        isAvailable = true;
    }

    public override void Interact()
    {
        GiveThisFood(customer.food_I_Want); // TODO 플레이어가 들고 있는 음식을 주도록 해야 한다!
    }

    // 테이블에 음식을 전달하면 그건 그대로 손님에게 전달된다.
    public void GiveThisFood(Food food){
        customer.GiveThisFood(food);
    }
}
