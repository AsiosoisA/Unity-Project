using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restaurant : MonoBehaviour
{

    public PlayerDataManager dataManager;
    private PlayerData playerData;

    /*
        식당의 업그레이드 정도에 따라 테이블 활성화되는게 달라짐!
    */
    public List<Table> level_0_tables;
    public List<Table> level_1_tables;
    public List<Table> level_2_tables;


    List<Table> myTables = new List<Table>();

    private int availableTableCount;
    
    public Menuboard menuboard;
    // Start is called before the first frame update

    public void Begin(){

        // 로딩이 끝나기 전엔 이 함수를 실행시키지 말 것!!
        this.playerData = dataManager.getPlayerData();

        Debug.Log("데이터 로딩 성공... 테이블 정보 초기화!");

        // 일단 기본적으로 테이블 정보를 초기화한다.
        myTables.AddRange(level_0_tables);
        if(playerData.restaurantLevel >= 1){
            myTables.AddRange(level_1_tables);
        }
        if(playerData.restaurantLevel >= 2){
            myTables.AddRange(level_2_tables);
        }
        // 이런 식으로 레벨 당 언락되는 상점 테이블을 추가해주면 되겠음.

        foreach(Table item in myTables){
            item.gameObject.SetActive(true);
        }

        availableTableCount = myTables.Count;

    }

    public Table chooseTable(){
        int[] availableTables = new int[availableTableCount];
        Debug.Log("가용한 테이블 수 : " + availableTableCount);

        int idx = 0;
        int myTableIdx = 0;

        foreach(Table table in myTables){
            if(table.isAvailable){
                Debug.Log("가용 가능하다고 판단된 테이블 번호 : " + table.tableNumber);
                availableTables[idx] = myTableIdx;
                idx++;
            }
            myTableIdx++;
        }
        // foreach 문으로 가용한 테이블을 알아낸 뒤, 그 테이블의 index 를 availableTables 에 저장했다. idx 에는 그 배열의 크기가 담겨 있음.

        Debug.Log("현재 가용한 테이블 수 : " + idx);

        int randIdx = Random.Range(0, idx); // 이 중 테이블 아무거나 하나 고른 뒤

        Table yourTable = myTables[availableTables[randIdx]];

        yourTable.isAvailable = false;
        availableTableCount--;

        return yourTable;
    }

    public int GetAvailableTableCount(){
        return this.availableTableCount;
    }
    public void PlusAvailableTableCount(){
        this.availableTableCount++;
    }
}
