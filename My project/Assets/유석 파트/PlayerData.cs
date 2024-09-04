using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using JetBrains.Annotations;

[System.Serializable]
public class PlayerDataForSave
{
    /*
        플레이어의 데이터를 저장할 클래스.


        어떤 데이터를 저장하게 하는 법 : 

            1. 변수를 public 으로 하나 선언한다.
            2. 밑에 있는 PlayerDataForJson 클래스에도 똑같이 public 으로 변수 하나를 선언한다. 이 때 {get; set;} 을 옆에 붙인다.
            3, 4. 두 클래스의 생성자를 이리조리 조작한다.
            
            5, 6. 추가로 슬롯을 통한 세이브 & 로드에서 미리보기 정보로 제공할 정보가 추가된다면 PlayerDataPreview 클래스에서도 조작해둘 것.

        이러면 PlayerDataManager 클래스가 알아서 데이터를 Save 하거나 Load 할 수 있게 된다!

        변경해야 하는 곳은 TODO 로 마킹을 해놨으니 예시를 보고 추가할 것.
    */



    // ====================================    공용 데이터    =========================================
    public int money;
    // ===============================================================================================






    // ====================================    RPG 데이터    =========================================
    //TODO 변경되어야 하는 곳!
    //추가 예시 : public int addedData;
    // ===============================================================================================






    // ====================================    식당 데이터    =========================================
    public int restaurantLevel;
    // ===============================================================================================
    
    

    // idea : 이 밖에도 여러 값들을 불러올 것. 이를 통해 현재 언락된 레시피가 무엇이 있는지 확인하겠음.

    // 혹은 언락된 레시피 목록을 인덱스만 배열로 저장하는 것도 나쁘진 않을 듯.
    













    ///////////     식당 레벨에 따라 동적으로 결정되는 값들     /////////////



    // PlayerData 를 생성자로 만드는 경우, 세이브 파일을 불러오는 경우. 즉 타임스탬프는 필요 없음.
    public PlayerDataForSave(PlayerDataForJSON data){
        
        //TODO 변경해야 하는 곳!
        //추가할 내용 : this.addedData = data.addedData;

        this.money = data.money;
        this.restaurantLevel = data.restaurantLevel;

        // 여기서부터 식당 레벨에 따라 동적으로 변경되는 값들 할당해주면 된다!
        
    }


    /*
        PlayerData 클래스를 PlayerDataForJSON 클래스로 일종의 압축을 하는 함수.
        이 함수엔 타임스탬프 값을 저장하는 기능도 탑재되어 있음!
    */
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
    public int timestamp_year { get; set; }
    public int timestamp_month { get; set; }
    public int timestamp_day { get; set; }
    public int timestamp_hour { get; set; }
    public int timestamp_minute { get; set; }
    public int timestamp_second { get; set; }






    //TODO 변경해야 하는 곳!
    //추가할 내용 : public int addedData { get; set; }

    public int money { get; set; }
    public int restaurantLevel { get; set; }

    public PlayerDataForJSON(){
        // Deserialize 를 위한 기본 생성자. 건들 필요 없음.
    }







    /*
        Simplificate 함수에 사용되는 생성자.

        일단 이 생성자도 데이터가 추가되면 변경해줘야 함.
    */
    public PlayerDataForJSON(PlayerDataForSave data){
        DateTime now = DateTime.Now;     
        timestamp_year = now.Year;
        timestamp_month = now.Month;
        timestamp_day = now.Day;
        timestamp_hour = now.Hour;
        timestamp_minute = now.Minute;
        timestamp_second = now.Second;


        //TODO 변경해야 하는 곳!
        //추가할 내용 : this.addedData = data.addedData;

        this.money = data.money;
        this.restaurantLevel = data.restaurantLevel;
    }
}

[Serializable]
public class PlayerDataPreview
{
    public int timestamp_year { get; set; }
    public int timestamp_month { get; set; }
    public int timestamp_day { get; set; }
    public int timestamp_hour { get; set; }
    public int timestamp_minute { get; set; }
    public int timestamp_second { get; set; }






    //TODO 변경해야 하는 곳!
    //추가할 내용 : public int addedData { get; set; }

    public int money { get; set; }
    public int restaurantLevel { get; set; }

    public PlayerDataPreview(){
        // Deserialize 를 위한 기본 생성자. 건들 필요 없음.
    }







    /*
        Simplificate 함수에 사용되는 생성자.

        일단 이 생성자도 데이터가 추가되면 변경해줘야 함.
    */
    public PlayerDataPreview(PlayerDataForJSON data){
        timestamp_year = data.timestamp_year;
        timestamp_month = data.timestamp_month;
        timestamp_day = data.timestamp_day;
        timestamp_hour = data.timestamp_hour;
        timestamp_minute = data.timestamp_minute;
        timestamp_second = data.timestamp_second;

        //TODO 변경해야 하는 곳!
        //추가할 내용 : this.addedData = data.addedData;

        this.money = data.money;
        this.restaurantLevel = data.restaurantLevel;
    }
}


