using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.U2D.Animation;

public class DefaultCustomerSpawner : MonoBehaviour
{


    /////////////////////////////////////  손님 객체를 찍어낼 때 쓸 변수들 ////////////////////////////////
    public GameObject defCustomerPrefeb; // 찍어낼 손님 객체의 프리팹.
    public GameObject storageToPutCustomer; // 찍어낸 손님이 담길 객체.
    private DefaultCustomer customer; // 찍어낸 손님을 담을 참조
    private Transform spawnPoint; // 찍어낼 손님의 위치
    /////////////////////////////////////////////////////////////////////////////////////////////////////



    
    /////////////////////////////////////  캐릭터 이름을 저장한 파일에서 샘플 이름 가져오기 /////////////////////////////////
    private List<string> maleNames;
    private List<string> femaleNames;
    private string maleNamesPath = "Assets/Resources/CustomerResources/MaleCustomerName.txt";
    private string femaleNamesPath = "Assets/Resources/CustomerResources/FemaleCustomerName.txt";
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



    ////////////////////////////////////// 손님 애니메이션을 위한 아틀라스 배열 만들기 ///////////////////////////////////////
    public List<SpriteAtlas> maleAtlasList;
    public List<SpriteAtlas> femaleAtlasList;

    // 여기서부터 이름들은 모두 임시 스프라이트에 맞춰져 있으나, 실제로는 손님 동작에 쓰일 이미지의 이름 형식에 맞춰 변경할 것
    // TODO :: 유지보수 :: 이 부분 애니메이션의 종류(아틀라스 형식)가 변경되면 수정되어야만 함!
    public string[] walkNames = {
        "player-idle-1" , 
        "player-idle-2" ,
        "player-idle-3" ,
        "player-idle-4"
    };

    public string[] waitNames = {
        "player-climb-1" , 
        "player-climb-2" ,
        "player-climb-3"
    };

    public string[] eatNames = {
        "player-hurt-1" ,
        "player-hurt-2"
    };
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////







    //////////////////////////////////    음식 결정을 위한 메뉴판    ////////////////////////////////////////////
    public Menuboard menuboard;
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////    


    void Awake()
    {
        /*
            이 객체는 시작될 때, 캐릭터의 샘플 이름을 저장한 파일에서 샘플 이름들을 가져와 List 로 저장한다.
        */
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
            result.Add(spawnCustomer());
        }

        return result; 
    }


    /*
        SpawnCustomer() 함수

        해당 함수로 초기화되는 것 : 

        1. 성별
        2. 이름
        3. 생김새 ( 애니메이션 & 아틀라스 )
        4. 포만도
        5. 기대 만족도

        이 값들이 초기화된 일반 손님 객체를 리턴함.
    */
    public DefaultCustomer spawnCustomer(){
        // 인스턴티에이트. 하이라키 창의 Customer Queue 객체 아래에 손님이 생성되도록 함.

        //Debug.Log("Spawner 발동!");

        spawnPoint = gameObject.transform; // 스폰포인트는 정확히 손님생성기가 있는 위치로 한다!
        customer = Instantiate(defCustomerPrefeb, spawnPoint.position, spawnPoint.rotation, storageToPutCustomer.transform).GetComponent<DefaultCustomer>();
        
        /*
            성별 결정
            Random 함수를 돌려서 0 이면 남자, 1이면 여자로 설정.
        */
        int gen = Random.Range(0, 2); // 0 ~ (2 - 1 인) 1 까지 랜덤으로 하나 지정.
        if(gen == 0) customer.gender = "male";
        else customer.gender = "female";

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
            손님 생김새, 즉 애니메이션 결정
            
            일단 이미 프리팹 레벨에서 애니메이션은 만들어져 있음.
            그 애니메이션에서 스프라이트만 바꾸면 된다!

            ex ) Walk - first frame = mySprite 이런 식으로 차자작 바꿔주자.
        */
        //Animator animator = customer.GetComponent<Animator>();

        List<SpriteAtlas> atlasList;
        if(customer.gender == "male"){
            atlasList = maleAtlasList;
            //Debug.Log("아틀라스 : 남성");
        }
        else{ // customer.gender == "female"
            atlasList = femaleAtlasList;
            //Debug.Log("아틀라스 : 여성");
        }

        // 우선 아틀라스 리스트에서 랜덤으로 아틀라스 하나를 가져온다.
        int atlasIdx = Random.Range(0, atlasList.Count);
        SpriteAtlas customerAtlas = atlasList[atlasIdx];

        // 아틀라스에서 이미지들을 추출한다.
        //TODO :: 유지보수 :: 현재 애니메이션 종류 : walk, wait , eat . 종류가 변경된다면 이 코드도 수정되어야 한다!
        Sprite[] walkSprites = GetSpritesFromAtlas(customerAtlas, walkNames);
        Sprite[] waitSprites = GetSpritesFromAtlas(customerAtlas, waitNames);
        Sprite[] eatSprites = GetSpritesFromAtlas(customerAtlas, eatNames);

        // 기존의 애니메이터를 기반으로 새 인스턴스를 생성한다.
        AnimatorOverrideController animOverrideController = new AnimatorOverrideController(customer.GetComponent<Animator>().runtimeAnimatorController);
        customer.GetComponent<Animator>().runtimeAnimatorController = animOverrideController;

        //TODO :: 유지보수 :: 현재 애니메이션 종류 : walk, wait , eat . 종류가 변경된다면 이 코드도 수정되어야 한다!
        /*
        ReplaceAnimationSprites(customer, "walk", walkSprites);
        ReplaceAnimationSprites(customer, "wait", waitSprites);
        ReplaceAnimationSprites(customer, "eat", eatSprites);
        */

        ReplaceAnimationSpritesOverride(animOverrideController, "walk", walkSprites);
        ReplaceAnimationSpritesOverride(animOverrideController, "wait", waitSprites);
        ReplaceAnimationSpritesOverride(animOverrideController, "eat", eatSprites);

        /*
            포만도 결정
            대충 0,1 중 랜덤하게 하나 결정. max = 2
        */
        customer.fullness = Random.Range(0, 2);



        /*
            기대 만족도 결정
            이것도 일단 나중에!
        */


        return customer;
    }


    /*
        GetSpritesFromAtlas 함수
        
        인자로 
        1. SpriteAtlas 하나
        2. 아틀라스 안에서 가져올 이미지들의 이름이 담긴 string 배열을 입력받는다.

        함수는 해당 이름 형식대로 스프라이트 배열을 생성하여 리턴해준다.
    */
    private Sprite[] GetSpritesFromAtlas(SpriteAtlas atlas, string[] spriteNames){
        Sprite[] sprites = new Sprite[spriteNames.Length];

        for(int i = 0; i < spriteNames.Length; i++){
            sprites[i] = atlas.GetSprite(spriteNames[i]);
            if(sprites[i] == null){
                Debug.LogError("GetSptiresFromAtlas : 어? 아틀라스에서 스프라이트를 가져오는 과정에서 뭔가 문제가 생겼다!");
            }
        }

        return sprites;
    }

    /*
        ReplaceAnimationSpritesOverride 함수
        애니메이터 오버라이드 컨트롤러를 쓰다 보니 클립 변경하는 방식을 좀 바꿔야 할 것 같아서 추가한 함수.
    */
    private void ReplaceAnimationSpritesOverride(AnimatorOverrideController animatorOverrideController, string clipName, Sprite[] newSprites){
        foreach(AnimationClip clip in animatorOverrideController.animationClips){ // 컨트롤러는 애니메이션 클립을 가지고 있다.
            if(clip.name == clipName){ // clip.name == walk 이런 식.
                
                AnimationClip newClip = new AnimationClip();
                EditorUtility.CopySerialized(clip, newClip);

                EditorCurveBinding[] bindings = AnimationUtility.GetObjectReferenceCurveBindings(newClip);
                foreach (var binding in bindings)
                {
                    ObjectReferenceKeyframe[] keyframes = AnimationUtility.GetObjectReferenceCurve(newClip, binding);
                    for (int i = 0; i < keyframes.Length; i++)
                    {
                        keyframes[i].value = newSprites[i % newSprites.Length];
                    }
                    AnimationUtility.SetObjectReferenceCurve(newClip, binding, keyframes);
                }

                animatorOverrideController[clip.name] = newClip;

                break;
            }
        }
    }
    
    /*
        ReplaceAnimationSprites 함수

        인자로
        1. 애니메이션을 변경할 손님 객체
        2. 클립 이름 ( walk , wait , eat 등 )
        3. 교체할 스프라이트 배열
        을 입력받는다.

        함수는 손님 객체의 애니메이션 컨트롤러에 들어 있는 클립 중 clipName 과 일치하는 클립의 스프라이트를 몽땅 교체한다!
    */
    private void ReplaceAnimationSprites(DefaultCustomer customer, string clipName, Sprite[] newSprites){
        //Animator animator = customer.GetComponent<Animator>();
        Animator animator = customer.animator;

        RuntimeAnimatorController controller = animator.runtimeAnimatorController; // 애니메이터의 애니메이션 컨트롤러를 얻는다.

        foreach(AnimationClip clip in controller.animationClips){ // 컨트롤러는 애니메이션 클립을 가지고 있다.
            if(clip.name == clipName) // clip.name == walk 이런 식.
            {
                ReplaceSprites(clip, newSprites); // 클립에서 쓰는 스프라이트를 교체한다. 자세한 로직은 ReplaceSprites 함수 주석을 참고하자!
                break;
            }
        }
    }

    /*
        ReplaceSprites 함수
        
        인자로
        1. 애니메이션 클립
        2. 교체할 스프라이트 배열
        을 입력받는다.

        함수는 입력받은 클립의 키프레임에 쓰이는 스프라이트들을 스프라이트 배열에 있는 스프라이트들로 교체해놓는다!
    */
    private void ReplaceSprites(AnimationClip clip, Sprite[] newSprites){
        EditorCurveBinding[] bindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);
        // EditorCurveBinding : 키프레임을 얻기 위한 열쇠(?). 애니메이션 클립이 특정 시점에 어떤 오브젝트를 참조하는지 정의해놓음.
        
        foreach(EditorCurveBinding binding in bindings)
        {
            ObjectReferenceKeyframe[] keyframes = AnimationUtility.GetObjectReferenceCurve(clip, binding);
            // GetObjectReferenceCurve 함수에 clip, binding 을 넣어 keyframe 을 얻어낸다!
            // 여기서 keyframe 의 value 에 기존 스프라이트가 담겨 있음.

            // 쩝... keyframes 를 얻기 위한 절차가 꽤 복잡하지만 어쩔 수 없다. 그러나 함수화해서 해치웠으니 걱정 말라!

            for(int i = 0; i < keyframes.Length; i++){
                keyframes[i].value = newSprites[i % newSprites.Length];
                // 이렇게 하는 이유는 두 가지가 있음.
                // 1. keyframes 의 개수가 더 적을 경우 : 알아서 잘 됨.
                // 2. keyframes 의 개수가 newSprites 의 개수보다 더 많을 경우 : newSprites의 맨 앞으로 돌아와서 부족한 키프레임을 채움.
            }

            AnimationUtility.SetObjectReferenceCurve(clip, binding, keyframes); // 키프레임 배열을 다시 애니메이션 클립에 채워넣는다!
        }
    }
}
