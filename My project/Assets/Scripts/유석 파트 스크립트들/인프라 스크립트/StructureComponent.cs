using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StructureComponent : MonoBehaviour, IInteractable
{
    /*
        PlayerInputHandler 로부터 OnInteractInput 메소드의 인풋 결과값 
        bool InteractInput
        값을 받아야 함.
    */

    protected bool InteractInput;
    public virtual void Interact(){}
    //public virtual void Interact(Player player){}

    protected virtual void Awake(){

    }
}
