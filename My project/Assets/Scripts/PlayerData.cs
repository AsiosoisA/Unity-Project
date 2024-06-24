using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class PlayerData
{
    /*
        플레이어의 데이터를 저장할 클래스.


        어떤 데이터를 저장하게 하는 법 : 

            1. 변수를 하나 선언한다.
            2. 밑에 있는 PlayerDataForJson 클래스에도 똑같이 변수 하나를 선언한다.
            3. 두 클래스의 생성자를 이리조리 조작한다.

        이러면 PlayerDataManager 클래스가 알아서 데이터를 Save 하거나 Load 할 수 있게 된다!
    */

    public int money;



    /*
        식당쪽 데이터
    */
    public int restaurantLevel;
    

    // 이 밖에도 여러 값들을 불러올 것. 이를 통해 현재 언락된 레시피가 무엇이 있는지 확인하겠음.

    // 혹은 언락된 레시피 목록을 인덱스만 배열로 저장하는 것도 나쁘진 않을 듯.
    
    





    ///////////     식당 레벨에 따라 동적으로 결정되는 값들     /////////////




    public PlayerData(PlayerDataForJSON data){
        this.money = data.money;
        this.restaurantLevel = data.restaurantLevel;

        // 여기서부터 식당 레벨에 따라 동적으로 변경되는 값들 할당해주면 된다!
        
    }

    public PlayerDataForJSON Simplificate(){
        return new PlayerDataForJSON(this);
    }

    



    ////////////////     플레이어 데이터를 업데이트하는 함수   ////////////////////
    public void AddMoney(int money){
        this.money += money;
    }
    public void SubMoney(int money){
        this.money -= money;
    }
    
}

[System.Serializable]
public class PlayerDataForJSON
{
    public int money;
    public int restaurantLevel;
    
    public PlayerDataForJSON(PlayerData data){
        this.money = data.money;
        this.restaurantLevel = data.restaurantLevel;
    }
}


