using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class USObjectPool : MonoBehaviour
{
    /*
        사용법!!!!!!!!!!

        아무 객체에나 이 컴포넌트를 붙인다.

        풀링하고 싶은 프리팹을 Prefab 에 할당
        적당히 넉넉한 사이즈를 InitialSize 에 할당
        Get 을 호출했을 때 객체가 쏘오옥 들어갈 목표 지점을 할당

        이래놓고 그냥 Get() 하면 프리팹이 리턴되고
        Release() 하면 프리팹을 돌려받는다.

        개 간단함!



        만약 Get 으로 얻어낸 객체에 대해 뭔가 작업을 하고 싶다?
        그럼 이걸 상속하게 새로운 오브젝트 풀을 하나 만들고

        InitInstance ( GameObject obj ) << 이거 override 해서 갓난아기 상태의 객체에 촤자자작 조작을 해뿌리면 됨! ㅎㅎ
    */





    //public static ObjectPool Instance;
    protected GameObject prefabGameObjectoPool;
    protected Queue<GameObject> objectPool = new Queue<GameObject>();

    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialSize = 16;

    [Header("객체가 생성될 하이라키 상의 목적지. \n할당하지 않아도 상관 없음.\n 할당되면 Get() 함수로 받은 인스턴스가\n 해당 목적지의 자식으로 들어감.")]
    [SerializeField] private GameObject targetToSendObjects;

    protected virtual void Awake()
    {
        Initialize(prefab, initialSize);
    }

    public void Initialize(GameObject prefab, int initialSize)
    {
        this.prefabGameObjectoPool = prefab;

        for(int i = 0; i < initialSize; i++)
        {
            GameObject newObject = GameObject.Instantiate(prefabGameObjectoPool);
            newObject.gameObject.SetActive(false);
            newObject.transform.SetParent(transform); // 새로 생성된 오브젝트는 하이라키 창에서 부모로 오브젝트 풀을 가진다.
            objectPool.Enqueue(newObject);
        }
    }

    public GameObject Get(GameObject destinationObject)
    {
        GameObject result;

        if(objectPool.Count > 0)
        {
            GameObject resultObject = objectPool.Dequeue();
            resultObject.gameObject.SetActive(true);
            resultObject.transform.SetParent(destinationObject.transform);

            result = resultObject;
        }
        else
        {
            GameObject newObject = GameObject.Instantiate(prefabGameObjectoPool);
            newObject.transform.SetParent(destinationObject.transform);

            result = newObject;
        }

        InitInstance(result);

        return result;
    }

    public virtual void InitInstance(GameObject obj){}

    public GameObject Get()
    {
        if(targetToSendObjects != null) return Get(targetToSendObjects);
        else return Get(null);
    }

    public virtual void Release(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(this.transform);
        objectPool.Enqueue(obj);
    }
}
