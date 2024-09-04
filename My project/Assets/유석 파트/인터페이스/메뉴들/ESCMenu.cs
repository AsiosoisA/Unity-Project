using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ESCMenu : InterfaceMenu
{
    public InterfaceMenu optionMenu;

    float originalTimeScale;

    protected override void Awake(){
        SetToggleKey(KeyCode.Escape);
    }

    public override void OnActive()
    {
        base.OnActive();
        originalTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    public override void OnInActive()
    {
        Time.timeScale = originalTimeScale;
        base.OnInActive();
    }

    /*
        ESC 를 눌렀을 때 나와야 하는 메뉴

        불러오기

        설정

        메인 메뉴로 나가기

        게임 종료

        저장 // 이건 글쎄다... 저장할 수 있는 기믹을 따로 만드는게 좋을 것 같다! ex ) 침대 등등
    */

    //불러오기가 눌렸을 때
    public void OnLoad(){
        
    }

    // 설정이 눌렸을 때
    public void OnSetting(){
        optionMenu.OnActive();
    }

    // 메인 메뉴로 나가기가 눌렸을 때
    public void OnExitMainMenu(){
        
    }

    // 게임 종료가 눌렸을 때
    public void OnTerminate(){
        //경고창 : 주의! 저장되지 않은 데이터는 사라질 것입니다. 정말 종료하시겠습니까?

        // Yes No
    }
}
