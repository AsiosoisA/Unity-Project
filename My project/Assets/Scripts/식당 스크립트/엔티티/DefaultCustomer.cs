using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultCustomer : Customer
{
    public override void InitCustomer()
    {
        /*

         본래 계획대로면 여기에서 온갖 것들을 초기화해야 했으나
         그것보다는 DefaultCustomerSpawner 클래스가 랜덤 이름, 아틀라스 등을 들고 있으면서 한꺼번에 초기화를 하는게
         손님마다 이 랜덤 이름을 다 들고 있는것보다 좋을 것 같아서 그 쪽으로 왠만한건 다 옮겼음.
        
        */

        // 여기에서는 일반 손님이 식당에 방문했을 때, 식당의 상태에 따라 유동적으로 변경되어야 하는 부분들을 초기화한다!

        /*
            고를 음식 결정
            메뉴판에 등록되어 있는 음식 중 하나를 랜덤으로 고른다!
        */

        this.food_I_Want = restaurant.menuboard.suggestFood();
 
        //Debug.Log(customerName + " 손님이 먹고 싶은 음식 : " + food_I_Want.foodName); // TODO
    }
}
