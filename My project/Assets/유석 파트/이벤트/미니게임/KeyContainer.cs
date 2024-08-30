using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyContainer : MonoBehaviour
{
    public float xOffset;
    public float yOffset;

    public float offsetBetweenItems;

    public float distanceBetweenItems;


    //Debug
    private Vector3 originalPosition;

    private Vector3 moveDestination;

    private bool isMoving;
    public float moveSpeed = 1f;


    #region 도구로 쓸 UI 아이템들
    public GuageBar guageBar;
    #endregion

    void Awake()
    {

    }

    void Update()
    {
        MoveContainer();
    }

    // Update is called once per frame
    public void SetPosition(GameObject requester, Vector3 offset)
    {
        if(requester == null) Debug.LogError("KeyContainer 에 requester가 아직 할당되지 않았습니다!");
        else Debug.Log("SetPosition 요청자 : " + requester.name);

        isMoving = false;
        transform.position = new Vector3(requester.transform.position.x + xOffset + offset.x, requester.transform.position.y + yOffset + offset.y, requester.transform.position.z);
        originalPosition = transform.position;
    }

    public void PlaceItems(List<KeyInstance> instances)
    {

        if(transform.childCount != 12) Debug.LogError("아이템을 배치하기도 전에 이미 아이템들이 있습니다!!!" + transform.childCount);

        distanceBetweenItems = GetDistanceBetweenItems(instances[0]);

        for(int i = 0 ;  i < instances.Count; i++)
        {
            instances[i].transform.SetParent(transform);

            Vector3 originalVector = transform.position;
            instances[i].transform.position = new Vector3(originalVector.x + i * distanceBetweenItems, originalVector.y, originalVector.z);
        }
    }

    public void PlaceJustOneItem(KeyInstance instance)
    {
        instance.transform.SetParent(transform);

        instance.transform.position = transform.position;
    }

    private float GetDistanceBetweenItems(KeyInstance instance)
    {
        return instance.keyBaseComponent.GetComponent<SpriteRenderer>().bounds.size.x + offsetBetweenItems;
    }

    public void MoveOneBlock()
    {
        if(isMoving)
        {
            isMoving = false;
            // 이동 중에 호출이 됐다!
            float valueShouldMove = transform.position.x - moveDestination.x;
            moveDestination = new Vector3(transform.position.x - distanceBetweenItems - valueShouldMove, transform.position.y, transform.position.z);
            isMoving = true;
        }
        else{
            moveDestination = new Vector3(transform.position.x - distanceBetweenItems, transform.position.y, transform.position.z);
            isMoving = true;
        }

    }

    private void MoveContainer()
    {
        if(isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveDestination, moveSpeed * Time.deltaTime);

            if(transform.position == moveDestination) isMoving = false;
        }
    }

    public void RestorePosition()
    {
        isMoving = false;
        transform.position = originalPosition;
    }
}
