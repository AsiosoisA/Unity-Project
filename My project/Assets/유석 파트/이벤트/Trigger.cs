using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

[System.Serializable]
public abstract class Trigger : MonoBehaviour
{
    public abstract void StartTrigger(); // Trigger 를 상속하는 클래스들에서 구현해야 하는 함수.
}