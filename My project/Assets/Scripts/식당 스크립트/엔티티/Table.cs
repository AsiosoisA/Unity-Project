using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Table : MonoBehaviour
{
    public int tableNumber;
    public bool isAvailable = true; // 손님이 앉으면 false로 바뀜.
    public Customer customer; // 테이블에 현재 앉아 있는 손님

    public string tableType; // 일단 나중을 위해 만들어 둠.

    public Restaurant myRestaurant;

    public void SitDown(Customer customer){
        this.customer = customer;

        /*
            의자의 sorting layer 를 그대로 따른다.
        */
        SpriteRenderer tableRenderer = GetComponent<SpriteRenderer>();
        int orderInLayer = tableRenderer.sortingOrder;
        customer.gameObject.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer;
    }
    public void GetUp(){
        customer = null;
        myRestaurant.PlusAvailableTableCount();
        isAvailable = true;
    }

    // 테이블에 음식을 전달하면 그건 그대로 손님에게 전달된다.
    public void GiveThisFood(Food food){
        customer.GiveThisFood(food);
    }
}
