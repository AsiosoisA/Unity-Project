using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkBulloon : MonoBehaviour
{
    private Animator animator;

    void Awake(){
        Debug.Log("말풍선 Awake");
        animator = GetComponent<Animator>();
    }

    // 이건 Default 라 딱히 설정할 필요가 없음.
    //public void ConsiderStart();

    public void OrderingStart(){
        animator.SetBool("isConsidering", false);
        animator.SetBool("isOrdering", true);
    }

    public void SatisfiedStart(){
        animator.SetBool("isOrdering", false);
        animator.SetBool("isSatisfied", true);
    }

    public void FuckStart(){
        animator.SetBool("isOrdering", false);
        animator.SetBool("isFuck", true);
    }
}
