using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using System.ComponentModel.Design.Serialization;
using UnityEditor.PackageManager.UI;

public class Restaurant : Infrastructure
{

    public SamplePlayer player;

    private static Restaurant instance;
    public static Restaurant Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<Restaurant>();

                if(instance == null)
                {
                    Debug.LogError("엥??? 레스토랑 객체를 찾을 수 없습니다!!");
                }
            }
            Instance = instance;
            return instance;
        }
        set
        {
            if(value == null)
            {
                Debug.LogError("레스토랑 인스턴스에 Null 을 할당하려고 시도중입니다!");
            }
            else instance = value;
        }
    }

    #region 비상호작용 컴포넌트들
    public Menuboard Menuboard { get; private set; }
    public RecipeBook recipeBook;
    #endregion
    
    #region 상호작용 가능한 가구들
    [SerializeField] private RestaurantComponents components; // 상호작용 가능한 레스토랑 컴포넌트들 모음집
    private List<Table> tables;
    public OrderList orderList;
    public Transform Doorway;
    #endregion

    #region 로직 데이터들
    private int availableTableCount;
    #endregion

    #region 유니티 메소드들

    public override void Awake(){

        // 원래는 테이블 레벨, 그릴 레벨, 칼 레벨 등등 모든 걸 다 따져서 각각 할당해야 하지만....
        // 지금은 그냥 식당 업그레이드 == 해당 레벨의 모든 컴포넌트들을 전부 얻을 수 있게 하자!

        Instance = this;
        Debug.Log("지금 가게 초기화됨!");

        components = GetComponentInChildren<RestaurantComponents>();
        Menuboard = transform.GetComponentInChildren<Menuboard>();
        
        InitDefaultComponents();
        InitTable();

        CheckIsAllInited();
    }
    #region Awake 서브로직
    private void InitDefaultComponents()
    {
        tables = components.level_1_tables;

        foreach(RestaurantComponent component in components.defaultComponentList)
        {
            component.UseThisToThisRestaurant(this);
        }
    }
    private void InitTable(){
        if(playerData.restaurantLevel >= 2){
            Init2LevelComponents();
        }
        if(playerData.restaurantLevel >= 3){
            Init3LevelComponents();
        }
        availableTableCount = tables.Count;

        foreach(Table item in tables){
            item.UseThisToThisRestaurant(this);
        }
    }

    private void Init2LevelComponents(){
        tables = tables.Concat(components.level_2_added_tables).ToList();
    }
    private void Init3LevelComponents(){
        tables = tables.Concat(components.level_3_added_tables).ToList();
    }
    private void CheckIsAllInited(){
        if(tables == null) Debug.LogError("Warning! 식당의 테이블이 초기화되지 않았어요."); 
    }
    #endregion

    protected override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    protected override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    #endregion

    #region 고유 메소드들
    public void GiveCustomerToTable(Customer customer){ // TODO 나중에 최적화할 수 있으면 최적화하기.
        int[] availableTables = new int[availableTableCount];
        //Debug.Log("가용한 테이블 수 : " + availableTableCount);

        int idx = 0;
        int myTableIdx = 0;

        foreach(Table table in tables){
            if(table.isAvailable){
                //Debug.Log("가용 가능하다고 판단된 테이블 번호 : " + table.tableNumber);
                availableTables[idx] = myTableIdx;
                idx++;
            }
            myTableIdx++;
        }
        // foreach 문으로 가용한 테이블을 알아낸 뒤, 그 테이블의 index 를 availableTables 에 저장했다. idx 에는 그 배열의 크기가 담겨 있음.

        //Debug.Log("현재 가용한 테이블 수 : " + idx);

        int randIdx = Random.Range(0, idx); // 이 중 테이블 아무거나 하나 고른 뒤

        Table yourTable = tables[availableTables[randIdx]];

        yourTable.SetOwner(customer);
        
        availableTableCount--;
    }
    public int GetAvailableTableCount(){
        return this.availableTableCount;
    }
    public void PlusAvailableTableCount(){
        this.availableTableCount++;
    }
    #endregion
}
