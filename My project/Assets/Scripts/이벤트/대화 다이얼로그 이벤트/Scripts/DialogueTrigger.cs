using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : Trigger
{
    public override void StartTrigger()
    {
        
        /*
            다이얼로그 이벤트는 트리거가 한 번 발동되면 다이얼로그를 출력한다.
        */

        transform.parent.gameObject.GetComponent<DialogueSystem>().Begin();

        
    }
}
