using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StructureComponent : MonoBehaviour
{
    /*
        PlayerInputHandler 로부터 OnInteractInput 메소드의 인풋 결과값 
        bool InteractInput
        값을 받아야 함.
    */


    public string structureName {get; protected set;}



    protected bool InteractInput;
    protected virtual void Awake(){

    }

    void Update() => LogicUpdate();

    void FixedUpdate() => PhysicsUpdate();

    protected virtual void LogicUpdate(){}
    protected virtual void PhysicsUpdate(){}


}
