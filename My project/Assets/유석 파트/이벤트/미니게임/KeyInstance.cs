using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KeyInstance : MonoBehaviour
{
    public string keyCodeName;
    private KeyCode keyCode;
    public EffectCore effectCore {get; private set;}

    public KeyInstanceComponent keyBaseComponent {get; private set;}
    public KeyCodeComponent keyCodeComponent {get; private set;}

    public SO_KeyCustom custom; // 이 값이 할당되지 않았다면 에러가 나야 한다.

    #region 상태값을 위한 불리언값들
    public bool isIdle{get; private set;}
    public bool isPushed{get; private set;}


    private bool isSensoring;

    public bool isFinished{get; private set;}
    private bool isLifeCycleEnded;
    #endregion

    // 나를 관리할 미니게임
    KeyMinigame myMinigame;

    public void Init(string keyCodeName, SO_KeyCustom custom) // 생성자처럼 쓸 것.
    {
        isIdle = true;
        isPushed = false;
        isSensoring = false;
        isFinished = false;
        isLifeCycleEnded = false;

        if(custom == null) Debug.LogError("KeyInstance에 대한 커스텀 스크립트를 먼저 넣어주세요!");
        else this.custom = custom;

        this.keyCodeName = keyCodeName;

        this.transform.localScale = new Vector3(custom.keySize, custom.keySize, custom.keySize);

        try
        {
            keyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyCodeName);
        }
        catch (System.ArgumentException)
        {
            Debug.LogError(keyCodeName + " 이라는 이름의 키와 대응하는 키코드를 찾을 수 없습니다.");
        }
        
        effectCore = GetComponentInChildren<EffectCore>();

        effectCore.Init();

        // KeyBase , KeyCode 컴포넌트는 각각 자식 클래스에서 내껄 초기화함.
    }

    void Update()
    {
        Sensoring();

        if(isPushed) myMinigame.OnKeyPressed();

        if(myMinigame != null) myMinigame.LogicUpdate();
    }
    

    public void StartSensoring(KeyMinigame watcher)
    {
        myMinigame = watcher;

        if(keyCodeName == null) Debug.LogError("먼저 KeyInstance.Init() 을 호출하여 키코드 이름을 할당해주세요!");

        isFinished = false;
        isSensoring = true;

        custom.DoEffects(custom.startEffects, effectCore);
    }

    public void Sensoring()
    {
        if(isSensoring)
        {
            if(Input.GetKeyDown(keyCode))
            {
                // 성공
                if(myMinigame.IsThisKeyUsing(keyCodeName)) 
                {
                    //Debug.Log("사용중인 키니까 그냥 스킵할게용~~ 입력 안 된걸로 쳐주세요!");
                    return; // 이 키가 사용중이라면 입력을 무시한다. 즉 처음부터 눌려있었으면 무시한다!
                }

                myMinigame.SetThisKeyUsing(keyCodeName);

                // 일단 눌림 애니메이션을 시작한다.
                isIdle = false;
                isPushed = true;

                // 눌렸을 때 일어날 추가적인 특수 효과를 이 곳에 작성한다.
                custom.DoEffects(custom.successEffects, effectCore);

                if(custom.isDisappearAfterSuccess)
                {
                    custom.DoEffects(custom.endEffects, effectCore);
                    isSensoring = false;
                    isFinished = true;
                }

                myMinigame.OnKeyInputSucessed();
                myMinigame.OnReadyToGetNextKey();
            }
            else if(Input.GetKeyUp(keyCode))
            {
                myMinigame.SetThisKeyNoLongerUsing(keyCodeName);

                isPushed = false;
                isIdle = true;

                myMinigame.OnKeyUped();
            }
            else if(Input.GetKeyDown(KeyCode.Escape))
            {
                // myMinigame.ShutDown();
            }
            else if(Input.anyKeyDown)
            {
                bool isPass = false;

                SO_KeyMinigame minigameData = myMinigame.GetData() as SO_KeyMinigame;
                foreach(string keyCodeItem in minigameData.keyList)
                {
                    if(Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), keyCodeItem)))
                    {
                        // 일단 이 키가 눌렸음은 확인!
                        if(myMinigame.IsThisKeyUsing(keyCodeItem))
                        {
                            //심지어 이미 쓰고 있는 녀석. 즉 전 키에 할당되었던 키코드 값이 눌려있는 상태일 뿐임.
                            isPass = true; // 휴 뭐야! 얜 실패 아니야~! 넘겨!
                        }
                    }
                }

                // 실패
                if(!isPass) custom.DoEffects(custom.failEffects, effectCore);
            }
        }
        else if(isFinished && Input.GetKeyUp(keyCode) && isPushed == true)
        {
            myMinigame.SetThisKeyNoLongerUsing(keyCodeName);

            isPushed = false;
            isIdle = true; // 정상화

            myMinigame.OnKeyUped();
        }
        else if(isFinished && !effectCore.isEffectHandling && !isLifeCycleEnded)
        {
            isLifeCycleEnded = true;
            myMinigame.OnKeyLifecycleFinished();
        }
    }

#region 다른 클래스에서 호출할 함수들
    public void SetBaseComponent(KeyInstanceComponent obj)
    {
        this.keyBaseComponent = obj;
    }
    public void SetKeyCodeComponent(KeyCodeComponent obj)
    {
        this.keyCodeComponent = obj;
    }
#endregion
}