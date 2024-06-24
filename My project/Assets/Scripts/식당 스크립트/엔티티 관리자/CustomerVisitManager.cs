using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
    이 클래스는 Begin() 함수가 발동되면 일정 주기마다 손님을 들여보낸다.

    만약 쓸 수 있는 테이블이 없다면 자리가 생길 때까지 대기시켰다가 바로 입장시킨다.


    이 클래스에 아래의 정보들이 들어 있음.

    1. 일일 손님 수
    2. 손님 방문 주기
    3. 줄 서다가 들어오는 손님 방문 주기

    해당 정보들은 현재 임시값을 쓰고 있음. 따라서 초기화를 반드시 해줘야 함!
*/
public class CustomerVisitManager : MonoBehaviour
{
    public Restaurant restaurant; // 식당
    public DefaultCustomerSpawner spawner; // 스포너

    private List<Customer> customerQueue = new List<Customer>(); // 손님 방문 순서.

    private int customerCount = 15; // 플레이어 데이터에서 일일 방문 손님 수를 얻어와야 함. 일단 임시값 사용.

    private float peroid = 1.0f; // 식당에 손님을 입장시킬 주기. (peroid) 초 마다 손님을 입장시키고, 꽉 차면 대기시킴.
    // 이거 일단 테스트하려고 값 설정했는데 나중엔 어차피 CalculatePeroid 함수에서 알아서 바뀜.
    private float waitingCustomersPeroid = 0.2f; // 기다리고 있던 손님들이 한꺼번에 들이닥칠 때의 주기.


    private int customerIdx = 0; // 현재 방문할 손님의 인덱스.
    private int waiting = 0; // 기다리고 있는 손님 수.

    public void Begin(){
        // 손님 리스트를 일단 만든다
        MakeCustomerQueue();

        // 손님을 들여보낼 주기를 계산한다. 하루 시간이 결정되면 그에 비례하여 입장시키는 시간도 적절히 설정할 것!
        CalculatePeroid();
        
        // 손님들을 코루틴으로 일정 시간마다 입장시킨다
        StartCoroutine(EnterCustomer(peroid));
    }

    public void MakeCustomerQueue(){
        customerQueue.AddRange(spawner.MakeCustomerList(customerCount));

        // 오늘 방문할 특별한 손님을 한 명 추가한다. 이 코드는 일단 나중에 짜자. // TODO 변경 가능!!!
        Customer specialGuest = InviteGuest();
        int randIdx = Random.Range(0, customerQueue.Count);
        customerQueue.Insert(randIdx, specialGuest);
    }

    public Customer InviteGuest(){
        // 조건에 맞는 손님 한 명을 여기에서 데려올 것!
        // 일단은 그냥 기본적인 손님 한 명 추가하겠음.
        return spawner.spawnCustomer();
    }

    public void CalculatePeroid(){
        // 원래 이 함수에서 종합적인 정보를 토대로 몇 초마다 손님을 방문시킬건지를 정해야 한다!
    }

    public IEnumerator EnterCustomer(float peroid){
        yield return new WaitForSeconds(3f); // 개장하자마자 바로 손님이 오면 당황스러우니 마음의 준비할 시간을 준다.

        while(customerIdx != customerQueue.Count){ // 손님을 모두 들여보낼 때까지 계속 된다!
            /*
                가용 가능한 테이블이 있는지, 아직 들어올 손님이 있는지 판단.

                가용 가능한 테이블이 없다면 대기열이 하나씩 늘어날거임.
            */

            while(restaurant.GetAvailableTableCount() == 0){
                if(customerIdx + waiting < customerQueue.Count) waiting++;
                Debug.Log("웨이팅 수 : " + waiting);

                /*
                    이제 기다리는데, 뭘 기다릴 것이냐.
                    1. 빈 자리가 생기는가?
                    2. 다음 손님이 올 때까지 빈자리가 생기지 않는가?

                    1. 빈자리가 생길 경우 while 문을 바로 탈출해야 함. => while 문의 조건으로 판별!
                    2. 빈자리가 생기지 않았는데 새 손님이 올 경우 => waiting 을 하나 더 추가.
                */
                float waitingTime = 0f;

                while(restaurant.GetAvailableTableCount() == 0){
                    waitingTime += 0.1f; // 0.1초

                    if(waitingTime >= peroid){
                        // 아차! 기다리는 동안 새 손님이 와버렸다...
                        waitingTime = 0f;
                        break;
                    }

                    yield return new WaitForSeconds(0.1f); // 0.1 초 기다린다.
                }
                // while 문을 탈출한 상태에서도 아직 손님이 앉을 자리가 없기 때문에 바로 다음 while 루프를 돌아 waiting이 증가됨.
            }

            // 가용 가능한 테이블이 생겼다!
            if(waiting != 0){

                // 기다리고 있던 손님들이 있다면?
                // 주기를 엄청 짧게 잡고, 손님들을 빠르게 들여보낸다.
                float tempPeroid = peroid;
                peroid = waitingCustomersPeroid;

                // 일단 쌓인 waiting 수를 모두 들여보냈을 때 오늘 기존에 방문하기로 한 손님 수를 초과하는지 검사해야 함.              
                while(waiting != 0 && customerIdx != customerQueue.Count && restaurant.GetAvailableTableCount() != 0){
                    
                    customerQueue[customerIdx++].Begin();
                    waiting--;
                    Debug.Log("웨이팅 수 : " + waiting);

                }
                // 이제 주기 정상화.
                peroid = tempPeroid;
            }
            else{
                customerQueue[customerIdx++].Begin();
            }

            if(customerIdx == customerQueue.Count) break; // 손님을 다 받았다면 while 문은 빠져나온다.

            yield return new WaitForSeconds(peroid); // 다음 손님을 받을 때까지 기다리기.
        }
    }
}
