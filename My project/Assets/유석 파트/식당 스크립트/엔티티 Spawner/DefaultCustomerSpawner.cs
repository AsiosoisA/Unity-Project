using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.U2D.Animation;

public class DefaultCustomerSpawner : MonoBehaviour
{


    /////////////////////////////////////  손님 객체를 찍어낼 때 쓸 변수들 ////////////////////////////////
    private Restaurant restaurant;
    public GameObject storageToPutCustomer; // 찍어낸 손님이 담길 객체.
    private DefaultCustomer customer; // 찍어낸 손님을 담을 참조
    private Transform spawnPoint; // 찍어낼 손님의 위치
    /////////////////////////////////////////////////////////////////////////////////////////////////////


    
    /////////////////////////////////////  캐릭터 이름을 저장한 파일에서 샘플 이름 가져오기 /////////////////////////////////
    private List<string> maleNames;
    private List<string> femaleNames;
    private string maleNamesPath = "Assets/Resources/유석 파트/CustomerResources/MaleCustomerName.txt";
    private string femaleNamesPath = "Assets/Resources/유석 파트/CustomerResources/FemaleCustomerName.txt";
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



    /// ///////////////////////////////////   손님 타입   /////////////////////////////////////////////////////
    private string[] customerTypes = {
        "Normal",
        "Warrior",
        "Archor",
        "Wizard",
        "Thief",
    };
    ////////////////////////////////////////////////////////////////////////////////////////////////////////



    ////////////////////////////////////// 컴포넌트들의 AC 를 바꿀 때 쓸 목록 ///////////////////////////////////////
    public SO_ACItemDictionary acItemDictionary;
    private Dictionary<string, List<SO_ACItem>> ACDictionary;
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    


#region 손님에게 적용할 파라미터값
    public float customerMaxWaitTime;
#endregion



    
    void Awake()
    {
        restaurant = Restaurant.Instance;
        if(restaurant == null) Debug.Log("가게 널임.");

        LoadCustomerNames();
   
        MakeDictionary();
        
    }
    private void MakeDictionary()
    {
        ACDictionary = new Dictionary<string, List<SO_ACItem>>();

        for(int i = 0; i < acItemDictionary.dictionaryKeys.Count; i++)
        {
            ACDictionary.Add(acItemDictionary.dictionaryKeys[i] , acItemDictionary.dictionaryItems[i].items);

            //Debug.Log("딕셔너리에 " + acItemDictionary.dictionaryKeys[i] + " 라는 이름의 키에 " + ACDictionary[acItemDictionary.dictionaryKeys[i]].Count + "개(" + ACDictionary[acItemDictionary.dictionaryKeys[i]][0].AC.name + "...)만큼의 AC를 할당했습니다.");
        }
    }

    public void LoadCustomerNames()
    {
        try{
            maleNames = new List<string>();
            femaleNames = new List<string>();
            string[] maleNamesArray = File.ReadAllLines(maleNamesPath);
            string[] femaleNamesArray = File.ReadAllLines(femaleNamesPath);
            // * AddRange : 배열을 List 에 추가하는 메소드.
            maleNames.AddRange(maleNamesArray);
            femaleNames.AddRange(femaleNamesArray);

            //Debug.Log("List 초기화 완료!");

        } catch (IOException e) {
            Debug.LogError("Error : 손님의 샘플 이름을 로딩하는 도중 뭔가 문제가 생겼음. : " + e.Message);
        }
    }

    /*
        MakeCustomerList 함수

        함수는 입력받은 count 수만큼 손님을 찍어낸 뒤 List<DefaultCustomer> 리스트에 넣어서 리턴해준다.
    */
    public List<DefaultCustomer> MakeCustomerList(int count){
        List<DefaultCustomer> result = new List<DefaultCustomer>();

        for(int i = 0; i < count; i++){
            DefaultCustomer item = spawnCustomer();
            result.Add(item);
        }

        return result; 
    }

    /*
        SpawnCustomer() 함수

        DefaultCustomer 에서 초기화해야 하는 부분은 모두 이 스크립트가 담당한다!!!

        이 값들이 초기화된 일반 손님 객체를 리턴함.
    */
    public DefaultCustomer spawnCustomer(){
        // 인스턴티에이트. 하이라키 창의 Customer Queue 객체 아래에 손님이 생성되도록 함.

        spawnPoint = restaurant.Doorway;
        customer = CustomerObjectPool.Instance.Get(storageToPutCustomer).GetComponent<DefaultCustomer>();

        customer.transform.position = spawnPoint.position;

        /*
            성별 결정
            Random 함수를 돌려서 0 이면 남자, 1이면 여자로 설정.
        */
        int gen = Random.Range(0, 2); // 0 ~ (2 - 1 인) 1 까지 랜덤으로 하나 지정.
        if(gen == 0) customer.gender = "male";
        else customer.gender = "female";

        //TODO 아직 여성 전용 체형이 마련되지 않았으니 반드시 남성으로 결정되도록 임의로 하드코딩!!!
        //TODOTODO @@@@@ 반  드  시  수  정  필  요  @@@@@ TODOTODO //
        customer.gender = "male";




        /*
            이름 결정
            샘플 이름들 중 성별에 맞게 하나 결정.
        */
        List<string> nameList;
        if(customer.gender == "male"){
            nameList = maleNames;
        }
        else{ // gender == "female"
            nameList = femaleNames;
        }
        
        if(nameList.Count == 0){
            Debug.LogError("DefaultCustomerSpawner : nameList 가 비어있는데요? 파일 다시 확인좀.");
        }
        else{
            int nameIdx = Random.Range(0, nameList.Count);
            customer.customerName = nameList[nameIdx];
        }



        /*
            타입 결정
            일반손님, 전사손님, 궁수손님, 도적손님, 마법사손님 중 하나를 결정.
        */
        customer.customerType = customerTypes[Random.Range(0, customerTypes.Length)];


        /*
            손님 생김새, 즉 애니메이터 컨트롤러 결정
        */
        SetComponentLook(customer, customer.customerBase);
        SetComponentLook(customer, customer.customerHairstyle);
        SetComponentLook(customer, customer.customerTop);
        SetComponentLook(customer, customer.customerBottom);
        SetComponentLook(customer, customer.customerOutfit);

        /*
            먹고 싶은 음식 결정
        */
        customer.food_I_Want = restaurant.Menuboard.suggestFood();


        /*
            포만도 결정
            0,1 중 랜덤하게 하나 결정. max = 2
        */
        customer.fullness = Random.Range(0, 2);


        /*
            손님이 최대로 기다릴 수 있는 시간 결정.
        */
        customer.patientTime = customerMaxWaitTime;

        //Debug.Log("성공적으로 손님 하나를 만들었습니다!");




        customer.gameObject.SetActive(false);

        return customer;
    }

    private void SetComponentLook(DefaultCustomer customer, CustomerComponent customerComponent)
    {
        string keyCodeToSearch = MakeSearchKey(customerComponent.componentName, customer);

        List<SO_ACItem> acItems = ACDictionary[keyCodeToSearch];

        if(acItems == null)
        {
            Debug.LogError("DefaultCustomerSpawner Error : keyCode " + keyCodeToSearch + "를 통해 딕셔너리에서 해당 AC를 찾을 수 없습니다.");
            return;
        }
        else if(acItems.Count == 0)
        {
            Debug.LogError("DefaultCustomerSpawner Error : keyCode " + keyCodeToSearch + "를 통해 기껏 찾은 리스트 개수가 0개입니다.");
            return;
        }
        else
        {
            /*
            Debug.Log("SetComponentLook : 성공적으로 리스트 로드에 성공했습니다.");
            foreach(SO_ACItem acItem in acItems)
            {
                Debug.Log("이름 : " + acItem.AC.name);
            }
            */
        }



        //TODO 여기서부터 다양하게 조건을 줄 수 있음.
        
        /*
            ex ) 베이스 컴포넌트의 외형으로 선정된 ACItem 을 기억했다가 다른 애들 선택할 때 조건을 준다던가.

            일단 지금은 그냥 랜덤으로 하나씩 고르는 로직을 넣어놓겠음.
        */



        SO_ACItem item = acItems[Random.Range(0, acItems.Count)];
        AnimatorOverrideController selected = item.AC;
        customerComponent.ChangeAC(selected);
    }

    private string MakeSearchKey(string partsName, DefaultCustomer customer)
    {
        string separator = "\\";
        return partsName + separator + customer.gender + separator + customer.customerType;
    }
}
