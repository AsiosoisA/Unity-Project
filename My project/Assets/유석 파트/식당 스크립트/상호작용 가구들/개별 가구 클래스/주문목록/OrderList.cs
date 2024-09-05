using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrderList : RestaurantComponent, IInteractableStructure
{
    public List<Order> orderQueue = new List<Order>();
    public List<Sprite> orderListSprites = new List<Sprite>();
    private SpriteRenderer spriteRenderer; 

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Order(Customer orderer, Food foodIWant)
    {
        Order newOrder = new(orderer, foodIWant)
        {
            recipe = restaurant.recipeBook.book[foodIWant.foodStuffName]
        };

        orderQueue.Add(newOrder);

        SinkSpriteWithListCount();
    }

    public void Interact(PlayerInteractState state, Player requester)
    {
        // 일단은 첫 번째 요소를 주자.
        if(orderQueue.Count == 0)
        {
            Debug.Log("주문목록에 주문이 아직 없습니다.");
            return;
        }

        Order item = orderQueue[0];
        orderQueue.RemoveAt(0);

        requester.restaurantInventory.holdingOrders.Add(item);

        SinkSpriteWithListCount();
    }

    public bool IsShouldHidePlayer()
    {
        return false;
    }

    private void SinkSpriteWithListCount()
    {
        int idx = orderQueue.Count;

        if(idx >= orderListSprites.Count) idx = orderListSprites.Count - 1;

        spriteRenderer.sprite = orderListSprites[idx];
    }

    public void RemoveMyOrder(Customer customer)
    {
        for(int i = 0 ; i < orderQueue.Count; i++)
        {
            if(orderQueue[i].orderer == customer)
            {
                orderQueue.RemoveAt(i);
            }
        }

        SinkSpriteWithListCount();
    }
}