using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D; // 2D 아틀라스를 위한 using 키워드.

/*

    손님 클래스는 """Begin()""" 함수가 실행되면 테이블을 골라서 앉고 음식을 주문, 먹고 나가는 등의 모든 행동을 순차적으로 수행한다!

*/



public abstract class Customer : MonoBehaviour // TODO 원래 abstract 여야 함.
{
    public int id; // 손님을 식별하기 위한 id
    public string gender; // 손님 성별
    public string customerName; // 손님 이름
    // 손님 이미지. Sprite 라고 써놓긴 했는데 아직 확실치 않음.
    // protected Animator animator;
    public Animator animator;
    public float patientGage = 100.0f; // 인내도

    public float patientTime = 10.0f; // 기다릴 시간
    float waitingTime = 0f; // 음식이 나오기까지 기다린 시간

       
    public Table myTable;
    public Food food_I_Want;


    public int fullness; // 포만감
    public float expected_satisfaction; // 기대 만족도

    
    public float actual_satisfaction; // 실제 만족도
    public int cost = 0; // 지불할 돈 액수
    public float revisiting_prob; // 재방문 확률




    // 손님 객체 이동을 위한 맴버변수들
    public float maxSpeed = 5.0f;
    //public Rigidbody2D rigid;
    public SpriteRenderer spriteRenderer;

    bool isGoingToTable = false;
    bool isWaiting = false;
    bool isExiting = false;

    private float outOfMapX = -12f; // 맵 밖 좌표. 이건 변경될 수 있음. 전역변수화하면 참 좋은데... 흠... TODO

    Transform tableTransform;



    public Restaurant restaurant;

    void Awake()
    {
        //rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();        
    }

    void FixedUpdate(){
        /*
            테이블이 할당되었을 때, 테이블로 가는 로직

            자기가 할당받은 테이블 위치까지 이동을 마치면 주문 ( Order ) 을 함.
        */

        /*
        if(isGoingToTable){
            float h = 1.0f;
            rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

            //Limiting Max Speed
            if(rigid.velocity.x > maxSpeed)
                rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);

            if(transform.position.x >= tableTransform.position.x){
                rigid.velocity = new Vector2(0, rigid.velocity.y);
                isGoingToTable = false;

                WaitBeforeOrder();
            }
        }
        */

        // rigidBody 가 손님에게 부여되는게 이상하기 때문에 그냥 없애고 코드 간략화함.
        if(isGoingToTable){
            gameObject.transform.Translate(Vector2.right * maxSpeed * Time.fixedDeltaTime);

            if(transform.position.x >= tableTransform.position.x){
                isGoingToTable = false;
                WaitBeforeOrder();
            }
        }

        /*
            밥 다 먹거나 안 먹거나 아무튼 나가는 로직.

            다 나가면 손님은 제거되어야 한다. 아님 다른 로직을 넣던가! TODO
        */

        /*
        if(isExiting){
            float h = 1.0f;
            rigid.AddForce(Vector2.left * h, ForceMode2D.Impulse);

            //Limiting Max Speed
            if(rigid.velocity.x < maxSpeed * (-1))
                rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
            
            if(transform.position.x <= outOfMapX){
                gameObject.SetActive(false); // 비활성화한다!
            }
        }
        */

        if(isExiting){
            gameObject.transform.Translate(Vector2.left * maxSpeed * Time.fixedDeltaTime);

            if(transform.position.x <= outOfMapX){
                gameObject.SetActive(false); // 비활성화한다!
            }
        }
    }

    void OnDisable() {
        Debug.Log("손님이 완전히 나갔음!");
    }


    public abstract void InitCustomer();

    /*
        손님에게 Begin 명령을 주면, 손님은 알아서 의자로 찾아가서 자기가 주문할 음식을 골라 주문한 뒤 기다린다!
        밑으로 내려가면서 순서대로 한 작업씩 수행함.
    */
    public void Begin(){
        InitCustomer();
        SitMyTable(); // 식당에 처음 들어오면 일단 테이블을 찾아서 앉는다.
    }
    
    public void SitMyTable(){
        
        myTable = restaurant.chooseTable();
        Debug.Log("내가 앉을 테이블을 정했음.");
        myTable.SitDown(this);
        tableTransform = myTable.GetComponent<Transform>();

        isGoingToTable = true; // 앉으면 FixedUpdate 를 이용해서 이동 로직을 수행한다.
    }

    public void WaitBeforeOrder(){

        animator.SetBool("isWalking", false);
        animator.SetBool("isWaiting", true); // 기다린다.

        Debug.Log(" ... ... 하는 말풍선"); // TODO

        float waitingSecond = Random.Range(2, 7); // 뭐 시킬지 고민하기
        StartCoroutine(ChooseMenu(waitingSecond));
    }

    public IEnumerator ChooseMenu(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        // 일단 고민을 한다.
        Order(); // 고민을 마치고 주문을 시킨다.
    }


    Coroutine waitMyFoodControl; // StartCoroutine 반환값을 여기다 저장. 이는 GiveThisFood 에 쓰임.
    public void Order(){
        // 이 시점에서 손님이 먹고 싶은 음식은 정해져야 함.
        // DefaultCustomer : 식당 방문할 때 (InitCustomer) 음식 하나를 정함.
        // SpecialCustomer : 얜 클래스 레벨에서 알아서 정해짐.

        isWaiting = true;

        waitMyFoodControl = StartCoroutine(WaitMyFood());
    }

    
    IEnumerator WaitMyFood(){

        Debug.Log("음식을 기다리는 중 ... ");
        while(waitingTime < patientTime){
            patientGage = Mathf.Lerp(100f, 0f, waitingTime/patientTime); // 100 부터 0까지 게이지를 감소시킨다.
            waitingTime += Time.deltaTime; // 대충 Update 간격만큼 초를 더함
            yield return null; // 한 프레임 기다림.
        }

        // 이 시점에서 waitingTime 이 patientTime 을 넘어섰음.
        patientGage = 0f;

        Fuck();
    }

    public void Fuck(){
        Debug.Log(" !$!@$%!%!^ 말풍선");
        animator.SetBool("isWaiting", false); // 그만 기다린다.
        ExitRestaurant();
    }

    /*
        외부에서 손님을 컴포넌트로 가지고 있을 때 호출하는 함수. 

        1.손님이 음식을 기다리는 중이며, 
        2. 자기가 원하던 음식일 경우 
        
        WaitMyFood를 끝냄.
    */
    public void GiveThisFood(Food food){
        if(isWaiting){
            if(food.foodName == food_I_Want.foodName){
                StopCoroutine(waitMyFoodControl);
                StartCoroutine(Eat());
            }
        }

        // 그 외에는 아무 일도 일어나지 않음.
    }

    IEnumerator Eat(){
        Debug.Log("먹는 효과음 출력 등 아무튼 먹는 중...");

        animator.SetBool("isWaiting", false); // 그만 기다린다.
        animator.SetBool("isEating", true); // 먹기 시작한다
        

        yield return new WaitForSeconds(5f); // 5초동안 야무지게 먹는다.

        /*
            여기에 만족도 판정을 넣을 것. 손님이 만족했다면 Satisfied , 불만족이라면 Fuck 을 호출.
        */

        Satisfied();
    }

    public void Satisfied(){ // 흠... 말풍선 스프라이트를 손님마다 머리 위에 매달아놓고, Update로 손님 머리 위에 항상 안주하도록 해볼까? << 좋은듯?
        Debug.Log(" 하트 말풍선 ");
        Debug.Log(" 돈 지급 ");
        ExitRestaurant();
    }

    public void ExitRestaurant(){

        // 돌아갈 때 의자는 반납한다!
        myTable.GetUp();

        animator.SetBool("isEating", false); // 그만 먹는다
        animator.SetBool("isWalking", true); // 이제 걷는다

        spriteRenderer.flipX = true; // 뒤집는다.

        isExiting = true; // 이거 활성화되면 Update 함수 레벨에서 제어됨.
    }




    // setter , getter

    



}

