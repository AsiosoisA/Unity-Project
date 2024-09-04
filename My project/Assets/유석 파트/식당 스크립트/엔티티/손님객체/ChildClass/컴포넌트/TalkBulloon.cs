using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkBulloon : MonoBehaviour
{
    private Animator animator;
    public SpriteRenderer foodSprite;

    void Awake(){
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isConsidering", isConsidering);
        animator.SetBool("isOrdering", isOrdering);
        animator.SetBool("isSatisfying", isSatisfying);
        animator.SetBool("isFucking", isFucking);
    }

    public bool isIdle;
    public bool isConsidering;
    public bool isOrdering;
    public bool isSatisfying;
    public bool isFucking;

    public void SetPosition(Transform target, float xOffset, float yOffset)
    {
        transform.position = new Vector3(target.position.x + xOffset, target.position.y + yOffset, target.position.z);
    }
}
