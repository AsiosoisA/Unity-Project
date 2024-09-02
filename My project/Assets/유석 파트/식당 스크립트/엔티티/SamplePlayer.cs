using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class SamplePlayer : MonoBehaviour
{
    public RestaurantInventory restaurantInventory;

    public new BoxCollider2D collider;





    public int debug_playerSliceLevel;
    public void SliceLevelUp() => debug_playerSliceLevel++;



    void Awake()
    {
        InitRestaurantInventory();
    }

    void InitRestaurantInventory()
    {
        this.restaurantInventory = GetComponentInChildren<RestaurantInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("상호작용!");
            Interact();
        }

        if(Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-1 * 5f * Time.deltaTime, 0f, 0f);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(1 * 5f * Time.deltaTime, 0f, 0f);
        }
    }

#region 상호작용 코드
    void Interact()
    {
        /*
            이 메소드가 발동되면 플레이어는 충돌한 Collider 를 감지한다.
            이 때 감지된 Collider들 중, IInteractive 인터페이스를 구현하는 객체들을 모아
            플레이어와 가장 많이 겹쳐진 녀석을 하나 고른다.
            마지막으로 그 녀석에 대해 Interact 함수를 호출한다!
        */
        
        Collider2D[] overlaps = Physics2D.OverlapBoxAll(collider.bounds.center , collider.bounds.size, 0f);

        List<IInteractable> interactables = new List<IInteractable>();

        IInteractable objShouldInteract = null;
        float maxXDistance = -1;

        foreach(Collider2D obj in overlaps)
        {
            IInteractable interactableObject = obj.gameObject.GetComponent<IInteractable>();
            if(interactableObject != null)
            {
                Debug.Log("상호작용 가능한 녀석을 하나 만났다! : " + obj.gameObject.name);

                Bounds intersection = GetIntersection(collider.bounds, obj.bounds);

                if(intersection.size != Vector3.zero)
                {
                    if(intersection.size.x > maxXDistance)
                    {
                        objShouldInteract = interactableObject;
                        maxXDistance = intersection.size.x;
                    }
                }
                else
                {
                    Debug.LogError("엑????? 플레이어의 Interact 로직에서 일어나면 안 되는 일이 일어남!"); // 실제로 일어나지 않아야만 함.
                }
            }
        }

        // 상호작용해야 하는 녀석을 발견했음.
        if(objShouldInteract != null) 
        {
            objShouldInteract.Interact(this.gameObject);
        }

        else Debug.Log("상호작용 가능한 물체가 없습니다.");
    }

    public void OnInteractFinished() // 그냥 IdleState 로 넘어가는 로직.
    {
        Debug.Log("플레이어가 상호작용을 성공적으로 마쳤습니다!");

        // 이제 여기서 State 전환하면 됨. 해야 할 일은 상호작용 가능한 객체에서 player 의 맴버를 싹 바꿔버리고 끝낼거임.
    }

    private Bounds GetIntersection(Bounds bounds1 , Bounds bounds2) // 상호작용할 객체를 선택할 때 면적을 계산하기 위한 함수.
    {
        Vector3 min = Vector3.Max(bounds1.min, bounds2.min);
        Vector3 max = Vector3.Min(bounds1.max, bounds2.max);

        // 두 바운드가 실제로 겹치는지 확인
        if (min.x < max.x && min.y < max.y)
        {
            // 겹치는 영역이 존재하는 경우, 그 영역의 중심과 크기를 반환
            return new Bounds((min + max) / 2, max - min);
        }

        // 겹치는 영역이 없다면 크기가 0인 빈 바운드를 반환
        return new Bounds(Vector3.zero, Vector3.zero);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
    }
#endregion


}
