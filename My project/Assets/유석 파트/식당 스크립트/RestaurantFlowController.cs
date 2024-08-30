using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantFlowController : MonoBehaviour
{
    public PlayerDataManager dataManager;
    public PlayerData playerData;
    public Customer tester;
    public Restaurant restaurant;
    public CustomerVisitManager visitManager; // 손님 방문 관리자



    public GameObject restaurantStartButton;





    // Start is called before the first frame update
    void Awake()
    {
        
    }
    void Start()
    {
        /*
            로딩이 끝날 때까지는 플레이어 데이터를 계속해서 기다린다!
        */
        StartCoroutine(WaitLoadingAndExec()); // 데이터가 감지되면 Begin 함수들을 다 실행한다!
    }

    IEnumerator WaitLoadingAndExec(){
        while(playerData == null){
            playerData = PlayerDataManager.Instance.Data;
            Debug.Log("데이터 불러오는 중...");
            yield return null; // 대기
        }

        if(playerData != null) Debug.Log("Data loading success!");
        else Debug.LogError("데이터 불러오는데 실패했습니다!");
        //tester.Begin();
    }

    public void RestaurantOpen()
    {
        restaurant.isOpened = true;
        visitManager.Begin();
        restaurantStartButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
