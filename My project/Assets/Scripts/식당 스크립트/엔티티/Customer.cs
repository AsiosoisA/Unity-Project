using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D; // 2D 아틀라스를 위한 using 키워드.

/*
    손님 클래스는 """Begin()""" 함수가 실행되면 테이블을 골라서 앉고 음식을 주문, 먹고 나가는 등의 모든 행동을 순차적으로 수행한다! (라이프사이클)
*/

public abstract class Customer : MonoBehaviour
{
    #region 손님 정보
        #region 손님 데이터
            public int id; // 손님을 식별하기 위한 id
            public string gender; // 손님 성별 [DefaultSpawner 에서 초기화됨]
            public string customerName; // 손님 이름 [DefaultSpawner 에서 초기화됨]
            public int fullness; // 포만감 [DefaultSpawner 에서 초기화됨]
            [SerializeField] private float maxSpeed = 5.0f; // 손님 속도
            [SerializeField] private float eatingTime = 5.0f; // 음식을 먹는데 걸리는 시간
            public float patientTime = 10.0f; // 음식을 기다려주는 최대 인내 시간
        #endregion

        #region 손님의 컴포넌트 (자동 할당)
            public Animator animator {get; protected set;} // 손님의 애니메이터
            public SpriteRenderer spriteRenderer {get; protected set;}
        #endregion

        #region 손님과 상호작용하는 객체들
            public Restaurant restaurant;
            protected Table myTable;
            protected Transform tableTransform;
            public Food food_I_Want;
            private float outOfMapX = -12f; // 출구 좌표. 이건 변경될 수 있음. 전역변수화하면 참 좋은데... 흠... TODO
            public TalkBulloon talkBulloon; // 말풍선 객체
        #endregion

        #region 로직 관련 정보
            
            private float patientGage = 100.0f; // 인내도
            private float waitingTime = 0f; // 음식이 나오기까지 기다린 시간

            bool isGoingToTable = false;
            bool isWaiting = false;
            bool isExiting = false;

        #endregion
    #endregion

    #region 초기화 파트
        void Awake()
        {
            //rigid = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            AwakeTalkBullon();
            
            InitCustomer();
        }

        private void AwakeTalkBullon(){
            MovePositionOfTalkBullonToHead(); // 말풍선의 위치를 이 손님의 머리 위로 옮긴다.
            talkBulloon.gameObject.SetActive(false); // 그리고 일단 숨겨놓는다.
        }

        private void MovePositionOfTalkBullonToHead(){
            float topOfCustomer = transform.position.y + spriteRenderer.bounds.extents.y;
            talkBulloon.bullonTransform.position = new Vector3(talkBulloon.bullonTransform.position.x, topOfCustomer + talkBulloon.bullonRenderer.bounds.size.y / 2 , talkBulloon.bullonTransform.position.z);
        }

        public abstract void InitCustomer(); // DefaultCustomer 의 경우 딱히 구현하지 않음.
    #endregion

    #region Update 파트
        void FixedUpdate(){ // 이동 로직을 굳이 손님마다 다르게 할 필요는 없을 것 같아서 그냥 여기다 냅다 때려박음.

            /*
                테이블이 할당되었을 때, 테이블로 가는 로직

                자기가 할당받은 테이블 위치까지 이동을 마치면 주문 ( Order ) 을 함.
            */

            if(isGoingToTable){
                gameObject.transform.Translate(Vector2.right * maxSpeed * Time.fixedDeltaTime);

                if(transform.position.x >= tableTransform.position.x){ // 테이블에 도착하면
                    isGoingToTable = false;
                    WaitBeforeOrder();
                }
            }

            /*
                밥 다 먹거나 안 먹거나 아무튼 나가는 로직.

                다 나가면 손님은 제거되어야 한다. 아님 다른 로직을 넣던가! TODO
            */

            if(isExiting){
                gameObject.transform.Translate(Vector2.left * maxSpeed * Time.fixedDeltaTime);

                if(transform.position.x <= outOfMapX){
                    CustomerExit();
                }
            }
        }
    #endregion

    /*
        라이프 사이클 : Begin > ChooseMyTable > GoToTable > WaitBeforeOrder > ChooseMenu > Order > WaitMyFood > 밥 대접에 실패했을 때 분기 / 밥 대접에 성공했을 때 분기 > ExitRestaurant > CustomerExit
        원래는 Finite State Machine 구조로 돌리는게 가장 이상적이지만, 재활용 가능성이 하나도 없는 손님 객체 특성상 그냥 이 구조 그대로 가겠음. 리팩토링하는데에 더 시간이 오래 걸릴 듯...
    */
    #region Customer 라이프사이클
    /*
        손님에게 Begin 명령을 주면, 손님은 알아서 의자로 찾아가서 자기가 주문할 음식을 골라 주문한 뒤 기다린다!
        밑으로 내려가면서 순서대로 한 작업씩 수행함.
    */
    public void Begin(){
        EnterEvent(); // 식당에 들어왔을 때 수행할 이벤트가 있다면 이벤트를 시작한다.
        ChooseMyTable(); // 식당에 처음 들어오면 일단 내가 앉을 테이블을 찾는다.
    }
    public void ChooseMyTable(){
        myTable = restaurant.chooseTable(); // 레스토랑한테 부탁해서 내가 앉을 식당 하나를 안내받음.
        myTable.SetCustomer(this); // 그 의자에 앉자.
        tableTransform = myTable.transform;
        GoToTable();
    }
    public void GoToTable(){
        isGoingToTable = true; // 앉으면 FixedUpdate 를 이용해서 이동 로직을 수행한다.
    }

    /* FixedUpdate 에 있는 코드
        if(isGoingToTable){
        gameObject.transform.Translate(Vector2.right * maxSpeed * Time.fixedDeltaTime);

        if(transform.position.x >= tableTransform.position.x){ // 테이블에 도착하면
            isGoingToTable = false;
            WaitBeforeOrder();
        }
    */
    public void WaitBeforeOrder(){

        AfterSitDownEvent();

        animator.SetBool("isWalking", false);
        animator.SetBool("isWaiting", true); // 기다린다.

        talkBulloon.gameObject.SetActive(true); // talkBullon 을 활성화하면 디폴트 애니메이션이 ... 이기 때문에 고민중인 말풍선이 자동으로 출력된다.

        float waitingSecond = Random.Range(2, 7); // 뭐 시킬지 고민하기
        StartCoroutine(ChooseMenu(waitingSecond));
    }
    public IEnumerator ChooseMenu(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        // 일단 고민을 한다. 실제로는 이미 먹고 싶은 음식이 정해져 있기 때문에 그냥 시간만 흐른다.
        Order(); // 고민을 마치고 주문을 시킨다.
    }
    Coroutine waitMyFoodControl; // StartCoroutine 반환값을 여기다 저장. 이는 GiveThisFood 에 쓰임.
    public void Order(){
        // 이 시점에서 손님이 먹고 싶은 음식은 정해져야 함.
        // DefaultCustomer : 식당 방문할 때 (InitCustomer) 음식 하나를 정함.
        // SpecialCustomer : 얜 클래스 레벨에서 알아서 정해짐.

        isWaiting = true;

        talkBulloon.OrderingStart();

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
        talkBulloon.FuckStart(); // !@#!@$!% 하는 말풍선이 나감

        animator.SetBool("isWaiting", false); // 그만 기다린다.
        ExitRestaurant();
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
        if(isWaiting && food.foodName == food_I_Want.foodName){
            StopCoroutine(waitMyFoodControl); // 기다림을 강제로 중지함.
            StartCoroutine(Eat()); // 먹기를 시작함.
        }
    }
    IEnumerator Eat(){
        EatEvent();
        talkBulloon.gameObject.SetActive(false); // 밥 먹을 땐 말풍선을 가린다.

        animator.SetBool("isWaiting", false); // 그만 기다린다.
        animator.SetBool("isEating", true); // 먹기 시작한다
        
        yield return new WaitForSeconds(eatingTime); // 5초동안 야무지게 먹는다.

        talkBulloon.gameObject.SetActive(true); // 다시 말풍선을 띄우자.
        Satisfied();
    }

    public void Satisfied(){
        talkBulloon.SatisfiedStart();     
        
        //Debug.Log(" 돈 지급 ");
        PlayerDataManager.Instance.GetData().AddMoney(food_I_Want.price);

        if(false){ // 만약 배가 아직 고픈 상태인데 매우 만족했다면 한 그릇 더 시킬 수도 있다! TODO 기능 추가하게 되면 여기에 추가할 것.

        }
        else ExitRestaurant();
    }
    #endregion

    public void ExitRestaurant(){

        // 돌아갈 때 의자는 반납한다!
        myTable.GetUp();

        animator.SetBool("isEating", false); // 그만 먹는다
        animator.SetBool("isWalking", true); // 이제 걷는다

        spriteRenderer.flipX = true; // 뒤집는다.

        isExiting = true; // 이거 활성화되면 Update 함수 레벨에서 제어됨.
    }
    /* Update 에 있는 isExiting 이후 로직
        if(isExiting){
            gameObject.transform.Translate(Vector2.left * maxSpeed * Time.fixedDeltaTime);

            if(transform.position.x <= outOfMapX){
                CustomerExit();
            }
        }
    */
    public virtual void CustomerExit(){
        ExitEvent();
        gameObject.SetActive(false); // 비활성화한다!
        // DefaultCustomer 에서 Override 해야 함. 즉, 오브젝트 풀에 넣는걸 여기서 해줄거임.
    }
    #endregion

    /*
        Special 손님의 경우 이 부분이 필요함.
        SpecialCustomer 클래스를 상속받는 각각의 특수 손님 객체에서 해당 함수들을 Override 한 뒤에 이 시점에 이벤트를 발생시키자!
    */
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
}