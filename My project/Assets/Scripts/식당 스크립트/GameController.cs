using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public PlayerDataManager dataManager;
    public PlayerData playerData;
    public Customer tester;
    public Restaurant restaurant;

    // Start is called before the first frame update
    void Start()
    {
        /*
            로딩이 끝날 때까지는 플레이어 데이터를 계속해서 기다린다!
        */

        dataManager.SyncThisData(playerData);
        StartCoroutine(WaitLoadingAndExec()); // 데이터가 감지되면 Begin 함수들을 다 실행한다!
    }

    IEnumerator WaitLoadingAndExec(){
        while(playerData == null){
            dataManager.SyncThisData(playerData);
            yield return null; // 대기
        }

        Debug.Log("Data loading success!");

        if(restaurant == null) Debug.Log("엥?????");
        restaurant.Begin();
        tester.Begin();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            Food food = new PigRiceSoup();

            tester.GiveThisFood(food);
            Debug.Log("음식을 전해줬음!");
        }
    }
}
