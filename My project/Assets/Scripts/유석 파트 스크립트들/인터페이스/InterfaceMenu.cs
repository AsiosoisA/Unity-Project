using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceMenu : MonoBehaviour
{
    public InterfaceManager manager; // 인터페이스의 매니저.
    public GameObject display; // 이 객체가 UI가 화면에 뿌려지고 안 뿌려지고를 담당한다.

    // 버튼 오브젝트들을 리스트화하고 위아래키로 SelectedButton 을 바꿀 수 있도록 기능 구현 해야겠다!


    protected KeyCode toggleKey; // 이 메뉴는 toggleKey 를 통해 껐다 킬 수 있음.
    bool isActive = false;
    bool isCheckInput = true;

    // 켜질 때 효과음
    // 꺼질 때 효과음

    void Awake(){
    }

    void Update(){
        ToggleInputCheck();
    }

    // =============  메뉴의 On , Off 를 위한 함수 =================
    protected void SetToggleKey(KeyCode code){
        this.toggleKey = code;
    }

    protected void IgnoreActiveInput(){
        this.isCheckInput = false;
    }

    protected void ToggleInputCheck(){
        if(isCheckInput && Input.GetKeyDown(toggleKey) && !isActive){
            // 키가 눌리면

            if(toggleKey == KeyCode.Escape){
                // 우연히도 켜는 키가 ESC 키였을 경우
                if(manager.menuStack.Count == 0){
                    
                    OnActive();
                }
                // 하나라도 창이 열려있는 경우, 해당 창을 끄는 목적으로 우선 양보한다.
            }
            else{ // ESC 키가 아니라 예외처리를 할 필요가 없으면 그냥
                OnActive(); // 켠다
            }
        }

        else if(Input.GetKeyDown(KeyCode.Escape) && isActive && !manager.isPushOrPoping){
            // 공통적으로 인터페이스 메뉴가 열려있는데 ESC 키가 눌린 경우 
            manager.menuStack.Peek().OnInActive(); // 끈다
            
            // 이 때 이 메뉴가 꺼지는 시점에 바로 밑의 메뉴도 덩달아 꺼지는 것을 막아야 함.
            StartCoroutine(Delay());
        }
    }

    public void OnActive(){
        isActive = true;
        // 켜질 때 효과음이 있다면 킬 것
        display.SetActive(true);

        manager.PushMenu(this);
    }
    public void OnInActive(){
        isActive = false;
        // 꺼질 때 효과음이 있다면 끌 것
        display.SetActive(false);

        manager.PopMenu();
    }

    IEnumerator Delay(){
        manager.isPushOrPoping = true;

        yield return new WaitForSeconds(0.01f);

        manager.isPushOrPoping = false;
    }

    // ===============================================================
}
