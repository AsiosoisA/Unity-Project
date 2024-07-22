using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    // 디버그 용이성을 위해 일단 public 으로 뒀음.
    public Stack<InterfaceMenu> menuStack = new Stack<InterfaceMenu>(); // 열려 있는 인터페이스들은 반드시 Top 에 있어야만 Update 메소드에서 키를 입력받을 수 있다.
    public bool isPushOrPoping = false;

    public void PushMenu(InterfaceMenu menu){
        if(isPushOrPoping) return;
        menuStack.Push(menu); // 입력받은 메뉴를 탑으로 올린다
    }

    public InterfaceMenu PopMenu(){
        if(isPushOrPoping) return null;

        InterfaceMenu menu = menuStack.Peek(); // 탑에 있는 놈을 확인한다
        menuStack.Pop(); // 실제로 탑에서도 제거시킨다.

        return menu; // 일단 꺼낸 녀석을 확인해본다.
    }
}
