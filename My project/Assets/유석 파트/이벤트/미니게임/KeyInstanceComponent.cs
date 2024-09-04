using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInstanceComponent : MonoBehaviour
{
    protected KeyInstance myKeyInstance;
    protected Animator animator;

    protected virtual void Awake()
    {
        myKeyInstance = GetComponentInParent<KeyInstance>();
        animator = GetComponent<Animator>();

        SetMeToMyKeyInstance();
    }

    protected virtual void SetMeToMyKeyInstance()
    {
        myKeyInstance.SetBaseComponent(this);
    }

    void Update()
    {
        animator.SetBool("isIdle", myKeyInstance.isIdle);
        animator.SetBool("isPushed", myKeyInstance.isPushed);
    }
}
