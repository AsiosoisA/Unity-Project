using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class Customer : MonoBehaviour
{
    #region 손님 정보
        #region 손님 데이터
            public int id; // 손님을 식별하기 위한 id
            public string gender; // 손님 성별 [DefaultSpawner 에서 초기화됨]
            public string customerName; // 손님 이름 [DefaultSpawner 에서 초기화됨]
            public int fullness; // 포만감 [DefaultSpawner 에서 초기화됨] << 여기서 초기화할 필요가 전혀 없음!!!!
            [SerializeField] private float maxSpeed = 5.0f; // 손님 속도
            [SerializeField] private float eatingTime = 5.0f; // 음식을 먹는데 걸리는 시간
            public float patientTime = 10.0f; // 음식을 기다려주는 최대 인내 시간
        #endregion

        #region 손님과 상호작용하는 객체들
            public Restaurant restaurant;
            protected Table myTable;
            public Food food_I_Want;
        #endregion

        #region 손님 컴포넌트
            public CustomerComponent customerBase;
            public CustomerComponent customerHairstyle;
            public CustomerComponent customerTop;
            public CustomerComponent customerBottom;
            public CustomerComponent customerOutfit;
            public List<CustomerComponent> customerComponents;


            public TalkBulloon talkBulloon;
        #endregion

        #region 로직 관련 정보
            
            private float patientGage = 100.0f; // 인내도
            private float waitingTime = 0f; // 음식이 나오기까지 기다린 시간

        #endregion

        #region 애니메이터 제어를 위한 boolean 값들
            public bool isGoingToTable = false;


            public bool isLookingSide = false;
            public bool isLookingFront = false;
            public bool isLookingBack = false;



            public bool isSitDowning = false;
            public bool isOrdering = false;
            public bool isWaiting = false;
            public bool isEating = false;
            public bool isStandUping = false;
    
            public bool isExiting = false;
        #endregion
    #endregion
    
    protected virtual void Awake()
    {
        restaurant = Restaurant.Instance;

        customerBase = GetComponentInChildren<CustomerBase>(); customerComponents.Add(customerBase);
        customerHairstyle = GetComponentInChildren<CustomerHairstyle>(); customerComponents.Add(customerHairstyle);
        customerTop = GetComponentInChildren<CustomerTop>(); customerComponents.Add(customerTop);
        customerBottom = GetComponentInChildren<CustomerBottom>(); customerComponents.Add(customerBottom);
        customerOutfit = GetComponentInChildren<CustomerOutfit>(); customerComponents.Add(customerOutfit);
        talkBulloon = GetComponentInChildren<TalkBulloon>();

        InitCustomer();
    }
    public virtual void InitCustomer()
    {

    }

    protected void Update()
    {
        LogicUpdate();
    }

    protected void FixedUpdate()
    {
        PhysicsUpdate();
    }
    protected virtual void LogicUpdate(){}
    protected virtual void PhysicsUpdate()
    {
        GoToTable();
        GoToExit();
    }

    #region 라이프사이클 메소드 
    public void Begin(){
        gameObject.SetActive(true);


        talkBulloon.isIdle = true;

        EnterEvent(); // 식당에 들어왔을 때 수행할 이벤트가 있다면 이벤트를 시작한다.
        Flip(); // 방향 바꿔야되더라.
        ChooseMyTable(); // 식당에 처음 들어오면 일단 내가 앉을 테이블을 찾는다.
    }

    public void ChooseMyTable(){
        //myTable = restaurant.chooseTable(); // 레스토랑한테 부탁해서 내가 앉을 식당 하나를 안내받음.
        //myTable.SetCustomer(this); // 그 의자에 앉자.
        
        restaurant.GiveCustomerToTable(this);
        
        isGoingToTable = true;
    }

    public void GoToTable()
    {
        if(isGoingToTable)
        {
            gameObject.transform.Translate(Vector2.left * maxSpeed * Time.fixedDeltaTime);

            if(transform.position.x >= myTable.transform.position.x)
            {
                isGoingToTable = false;
                SitDown();
            }
        }
    }

    public void SitDown()
    {
        SetSortingOrder(myTable.GetComponent<SpriteRenderer>().sortingOrder + 1); // 앞으로 꺼낸다

        FlipByTableType();

        transform.position = new Vector3(transform.position.x + myTable.customerPositionOffset, transform.position.y , transform.position.z);

        isSitDowning = true;
    }

    public void SitDownFinishTrigger()
    {
        isSitDowning = false;
        ThinkBeforeOrder();
    }

    public void ThinkBeforeOrder()
    {
        talkBulloon.isIdle = false;
        talkBulloon.isConsidering = true;

        AfterSitDownEvent();
        isWaiting = true;

        float thinkSecond = Random.Range(4,8); // 고민하는 시간
        StartCoroutine(Think(thinkSecond));
    }

    public IEnumerator Think(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Order();
    }

    Coroutine waitMyFoodControl; // StartCoroutine 반환값을 여기다 저장. 이는 GiveThisFood 에 쓰임.
    public void Order()
    {
        talkBulloon.isConsidering = false;
        talkBulloon.isOrdering = true;
        talkBulloon.foodSprite.sprite = food_I_Want.foodSprite;

        isWaiting = true;

        restaurant.orderList.Order(this, food_I_Want);
        
        waitMyFoodControl = StartCoroutine(WaitMyFood());
    }

    IEnumerator WaitMyFood(){
        while(waitingTime < patientTime){
            patientGage = Mathf.Lerp(100f, 0f, waitingTime/patientTime); // 100 부터 0까지 게이지를 감소시킨다.
            waitingTime += Time.deltaTime; // 대충 Update 간격만큼 초를 더함
            yield return null; // 한 프레임 기다림.
        }

        // 이 시점에서 waitingTime 이 patientTime 을 넘어섰음.
        patientGage = 0f;
        Fuck();
    }

    #region 밥 대접에 실패했을 때 분기
    public void Fuck(){
        FuckEvent();
        //talkBulloon.FuckStart(); // !@#!@$!% 하는 말풍선이 나감
        talkBulloon.isOrdering = false;
        talkBulloon.isFucking = true;
        talkBulloon.foodSprite.sprite = null;



        // !@$!@$ 하고 나갔을 때 되어야 하는 것 : 

        /*
            1. 주문 목록에 아직 이 사람이 시킨게 남아 있다면 제거해야 한다.
            2. 플레이어가 내 주문을 들고 있다면 이제 만들지 말라고 해야 한다. 
        */
        restaurant.orderList.RemoveMyOrder(this);
        restaurant.player.restaurantInventory.RemoveMyOrder(this);



        isWaiting = false;
        StandUp();
    }
    #endregion

    #region 밥 대접에 성공했을 때 분기
    /*
        외부에서 손님을 컴포넌트로 가지고 있을 때 호출하는 함수. 

        1.손님이 음식을 기다리는 중이며, 
        2. 자기가 원하던 음식일 경우 
        
        WaitMyFood를 끝냄.
    */
    public void GiveThisFood(Food food){
        if(isWaiting && food.foodStuffName == food_I_Want.foodStuffName){
            StopCoroutine(waitMyFoodControl); // 기다림을 강제로 중지함.
            StartCoroutine(Eat()); // 먹기를 시작함.
        }
    }
    IEnumerator Eat(){
        EatEvent();
        //talkBulloon.gameObject.SetActive(false); // 밥 먹을 땐 말풍선을 가린다.

        talkBulloon.isOrdering = false;
        talkBulloon.isIdle = true;
        talkBulloon.foodSprite.sprite = null;

        isWaiting = false;
        isEating = true;
        
        yield return new WaitForSeconds(eatingTime); // 5초동안 야무지게 먹는다.

        // talkBulloon.gameObject.SetActive(true); // 다시 말풍선을 띄우자.
        Satisfied();
    }

    public void Satisfied(){
        Debug.Log("와우! 대만족!");

        talkBulloon.isIdle = false;
        talkBulloon.isSatisfying = true;
        
        //Debug.Log(" 돈 지급 ");
        //PlayerDataManager.Instance.Data.AddMoney(food_I_Want.price);

        restaurant.todaySales += 10; // 일단 이렇게만 하자!

        if(false){ // 만약 배가 아직 고픈 상태인데 매우 만족했다면 한 그릇 더 시킬 수도 있다! TODO 기능 추가하게 되면 여기에 추가할 것.

        }
        //else ExitRestaurant();
        else StandUp();
    }
    #endregion

    public void StandUp()
    {
        isEating = false;
        isStandUping = true;
    }

    public void StandUpFinishTrigger()
    {
        SetSortingOrder(0); // 뒤로 보낸다
        isStandUping = false;
        ExitRestaurant();
    }

    public void ExitRestaurant()
    {
        FlipByTableType();

        isExiting = true;

        myTable.UnSetOwner(); // 손님은 테이블을 반납한다.

        Flip(); // 왔던 길로 돌아가기 위해 Flip 수행.
    }

    public void GoToExit()
    {
        if(isExiting)
        {
            gameObject.transform.Translate(Vector2.left * maxSpeed * Time.fixedDeltaTime);

            if(transform.position.x <= restaurant.Doorway.position.x){
                CustomerExit();
            }
        }
    }

    public virtual void CustomerExit()
    {
        ExitEvent();
        
        restaurant.OnCustomerExit();

        CustomerObjectPool.Instance.Release(this.gameObject);
    }

    #endregion

    #region 물리 동작 관련 메소드
    public void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        talkBulloon.transform.Rotate(0f, 180f, 0f); // 얜 플립 안 시킬거임.
    }
    #endregion

   #region 이벤트
        public virtual void EnterEvent(){ // 식당에 처음 들어왔을 때 실행될 이벤트

        }
        public virtual void AfterSitDownEvent(){ // 식당 의자에 앉았을 때 실행될 이벤트

        }
        public virtual void FuckEvent(){ // 음식을 받지 못 했을 때 화를 내는 이벤트

        }
        public virtual void EatEvent(){ // 음식을 먹기 시작할 때 이벤트

        }
        public virtual void ExitEvent(){ // 식당을 완전히 나갔을 때 실행될 이벤트

        }
    #endregion

    #region 개별 메소드
    public void SetMyTable(Table table)
    {
        this.myTable = table;
    }
    public void FlipByTableType()
    {
        if(myTable.tableChairLookDirection == Table.LEFT)
        {
            Flip();
        }

        if(myTable.tableChairLookDirection == Table.LEFT || myTable.tableChairLookDirection == Table.RIGHT)
        {
            isLookingSide = !isLookingSide;
        }
        else if(myTable.tableChairLookDirection == Table.FRONT)
        {
            isLookingFront = !isLookingFront;
        }
        else if(myTable.tableChairLookDirection == Table.BACK)
        {
            isLookingBack = !isLookingBack;
        }
    }
    public void SetSortingOrder(int order)
    {
        foreach(CustomerComponent component in customerComponents)
        {
            if(component.componentName == "base") {
                component.gameObject.GetComponent<SpriteRenderer>().sortingOrder = order - 1;
            }
            else if(component.componentName == "outfit")
            {
                component.gameObject.GetComponent<SpriteRenderer>().sortingOrder = order + 2;
            }
            else if(component.componentName == "bottom")
            {
                component.gameObject.GetComponent<SpriteRenderer>().sortingOrder = order + 1;
            }
            else component.gameObject.GetComponent<SpriteRenderer>().sortingOrder = order;
        }
    }
    #endregion
}
