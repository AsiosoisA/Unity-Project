using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerComponent : MonoBehaviour
{
    public string componentName {get; protected set;}
    public Customer MyCustomer {get; private set;}
    public Animator MyAnimator {get; private set;}

    protected virtual void Awake()
    {
        MyCustomer = GetComponentInParent<Customer>();
        MyAnimator = GetComponent<Animator>();
    }

    protected void Update()
    {
        LogicUpdate();
    }

    protected void FixedUpdate()
    {
        PhysicsUpdate();
    }

    protected virtual void LogicUpdate()
    {
        AnimatorUpdate();
    }

    protected virtual void PhysicsUpdate(){}

    protected virtual void AnimatorUpdate()
    {
        MyAnimator.SetBool("isWalking" , (MyCustomer.isGoingToTable || MyCustomer.isExiting));

        MyAnimator.SetBool("isLookingSide" , MyCustomer.isLookingSide);
        MyAnimator.SetBool("isLookingFront" , MyCustomer.isLookingFront);
        MyAnimator.SetBool("isLookingBack" , MyCustomer.isLookingBack);
        
        MyAnimator.SetBool("isSitDowning", MyCustomer.isSitDowning);
        MyAnimator.SetBool("isOrdering", MyCustomer.isOrdering);
        MyAnimator.SetBool("isWaiting", MyCustomer.isWaiting);
        MyAnimator.SetBool("isEating", MyCustomer.isEating);
        MyAnimator.SetBool("isStandUping", MyCustomer.isStandUping);
    }

    public void ChangeAC(AnimatorOverrideController newController)
    {
        if(newController == null) Debug.LogError("Warning!!! null 인 컨트롤러를 컴포넌트에 반영중입니다!");
        MyAnimator.runtimeAnimatorController = newController;
    }
}
