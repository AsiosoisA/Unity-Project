using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{

    // ======================  중요! 원래는 Option 메뉴에서 가져와야 하는 정보 ====================
    // 즉 나중에는 따로 Option 을 이용해서 초기화해줄 것. 지금은 임시값으로 하겠음.
    private float talkTerm = 0.045f; // 말하는 사람이 말을 하는 속도
    // =========================================================================================





    // ======================  다이얼로그를 출력하기 위해 설정해야 하는 뷰 =========================
    [Header("대화 다이얼로그 몸체")]
    public GameObject dialogueBox;
    [Header("말하는 이")]
    public Text talkerName;
    [Header("인물 대화")]
    public Text talkerContent;
    // ==========================================================================================



    // ========================  대화 스킵, 대화 넘기기 기능을 위한 값들  ==========================
    bool isSkip = false;
    bool isEndDialogue = false;
    Button button;
    Coroutine currentEffect;
    // ==========================================================================================




    /*
        DialogueDataManager 에서 가져온 string[] 값이 있을 때, 이걸 다이얼로그 시스템에 넣고 동작시키는 부분
    */
    //private Dialogue dialogue;
    private DialogueDataManager dataManager;
    private string[] sentences;
    private int sentenceIdx;
    private int talkerId;

    public void TalkStart(int talkerId, int questId, int questStat, int sentenceId){
        TalkStartBegin();
        this.talkerId = talkerId;

        //Debug.Log("DialogueSystem : TalkStart(" + talkerId + "_" + questId + "_" + questStat + "_" + sentenceId + ")");

        sentences = dataManager.GetTalk(talkerId, questId, questStat, sentenceId);
        //StartCoroutine(AfterGetTalk());
        AfterGetTalk();
    }
    // talkerId , questId 는 각각 원래의 string 이름이 올 수 있음.
    #region Overloading
    public void TalkStart(string talkerName, string questName, int questStat, int sentenceId){
        TalkStartBegin();
        TalkStart(dataManager.GetId(DialogueDataManager.objDict, talkerName) , dataManager.GetId(DialogueDataManager.questDict, questName), questStat, sentenceId);
    }
    public void TalkStart(string talkerName, int questId, int questStat, int sentenceId){
        TalkStartBegin();
        TalkStart(dataManager.GetId(DialogueDataManager.objDict, talkerName) , questId, questStat, sentenceId);
    }
    public void TalkStart(int talkerId, string questName, int questStat, int sentenceId){
        TalkStartBegin();
        TalkStart(talkerId , dataManager.GetId(DialogueDataManager.questDict, questName), questStat, sentenceId);
    }
    #endregion

    private void TalkStartBegin(){
        if(dataManager == null) Init();
    }

    void AfterGetTalk(){
        sentenceIdx = 0;
        Begin();
    }

    private void Init(){
        dataManager = GameManager.Instance.dialogueDataManager;
        if(dataManager == null) Debug.LogError("DialogueSystem : Error! 데이터 매니저를 불러올 수 없음.");
        //Debug.Log("dataManager 불러오기 성공");
    }


    public void Begin(){ // Trigger 로 시작되는 부분!
        dialogueBox.SetActive(true); // 다이얼로그를 화면에 노출시킨다!

        button = dialogueBox.gameObject.GetComponent<Button>(); // button : 다이얼로그 몸체의 버튼 컴포넌트
        button.onClick.RemoveAllListeners(); // 버튼의 리스너를 초기화한다.
        button.onClick.AddListener(OnDialoguePressed); // 버튼에 리스너를 달아준다.

        //talkerName.text = dialogue.name; // 일단 화자의 이름을 설정한다.
        talkerName.text = dataManager.GetName(DialogueDataManager.objDict, talkerId);

        // ====================   대화 한 글자식 출력하기   =====================
        talkerContent.text = ""; // 기존 텍스트는 비운다.
        StartCoroutine(TypeSentence());
        // ====================================================================
    }

    public void End(){
        isSkip = false;
        isEndDialogue = false;
        dialogueBox.SetActive(false);

        int lastIdx = sentences.Length - 1;
        if(lastIdx != sentenceIdx){
            sentenceIdx++;
            Begin();
        }
    }



    // ========================  대화 스킵, 대화 넘기기 기능  ==========================
    void Update(){

        /* 엔터키를 입력받았을 때
            1. 대화를 스킵하는 상황일 때
            2. 다음 대화로 넘어가고 싶을 때
        */
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)){
            OnDialoguePressed();
        }


    }

    public void OnDialoguePressed(){
        
        if(!isSkip){
            isSkip = true;

            if(currentEffect != null) StopCoroutine(currentEffect);
        } // 스킵중이 아닐 땐 스킵하기를 한다.

        if(isEndDialogue){
            End();
        } // 다이얼로그가 끝났다면 다이얼로그를 종료한다.
    }
    // ==========================================================================================



    bool isHandling = false; // TypeSentence 함수에서 특수 문자를 처리중인지를 확인하기 위한 boolean 값.
    string command; // TypeSentence 함수에서 얻어낸 특수 명령어.
    IEnumerator TypeSentence(){

        //foreach(var letter in dialogue.content){
        foreach(var letter in sentences[sentenceIdx]){
            // ===================== 특수 커맨드가 입력됐을 때는 이를 처리해줘야 함!!! ==========================
            if(letter == '<' && !isHandling){ // 특수 커맨드 입력이 인식되면
                isHandling = true; // 처리중... 상태로 만든다
            }
            else if(isHandling){
                if(letter == '>'){ // 특수 커맨드 입력 종료
                    isHandling = false;
                    
                    yield return StartCoroutine(CommandHandler(command));
                }
                else command += letter;
            }
            // ==============================================================================================


            /* 만약 그냥 평범한 대사가 입력되면 */
            else{
                talkerContent.text += letter;

                if(isSkip) continue;
                yield return new WaitForSeconds(talkTerm);
            }
        }

        // 중간에 방해 없이 모든 대화를 출력하는데 성공했다면
        isEndDialogue = true;
    }


    List<string> argv = new List<string>(); // 명령어의 인자 수
    int idx = 0;
    IEnumerator CommandHandler(string command){
        // command 문자열에서 인자들을 추출해 argv[] 에 집어넣는다!
        argv.Add(""); // 일단 첫 번째 아이템 초기화.

        foreach(var c in command){
            if(c == '(' || c == ','){
                argv.Add("");
                idx++;
            }
            else if(c == ')'){
                // 커맨드가 끝났다!
                break;
            }
            else{
                argv[idx] += c;
            } 
        }

        /*
            주의! isSkip 값에 따라 해당 로직들은 달라질 수 있음. 그 예외처리를 철저히 할 것!  
        */

        // 입력받은 인자를 바탕으로 연출 명령을 수행한다.
        if(argv[0] == "sleep"){

            float sec = float.Parse(argv[1]);
            Debug.Log("DialogueSystem : Sleep for " + sec + "Seconds.");

            if(isSkip) sec = 0f; // isSkip 값에 따른 처리.

            EndEffect();

            yield return StartCoroutine(WaitSeconds(sec));
        }


        else if(argv[0] == "color"){
            string colorHex;
            if(argv[1] == "red"){
                colorHex = ColorUtility.ToHtmlStringRGBA(Color.red);
            }
            else if(argv[1] == "green"){
                colorHex = ColorUtility.ToHtmlStringRGBA(Color.green);
            }
            else{
                Debug.LogError("해당 색깔은 아직 구현되지 않았습니다! 개발자에게 문의하십시오.");
                colorHex = ColorUtility.ToHtmlStringRGBA(Color.black);
            }

            string textToType = argv[2]; 

            EndEffect();

            yield return StartCoroutine(TypeColoredText(colorHex, textToType));
        }


        else{
            Debug.Log("DialogueSystem : Warning... Unknown Command Inputed!");
        }
    }

    // =================== 연출을 위한 함수들 ==========================
    IEnumerator WaitSeconds(float seconds){    
        float curSec = seconds;

        while(curSec > 0 && !isSkip){
            yield return null;
            curSec -= Time.deltaTime;
        }
    }

    IEnumerator TypeColoredText(string colorHex, string text){

        string coloredString;

        foreach(var newLetter in text){
            string stringLetter = "" + newLetter;

            coloredString = $"<color=#{colorHex}>{stringLetter}</color>";

            talkerContent.text += coloredString;

            if(isSkip) continue;
            yield return new WaitForSeconds(talkTerm);
        }
    }


    // 연출 끝났을 때 항상 이 함수를 실행할 것!
    void EndEffect(){
        argv.Clear();
        idx = 0;
        command = string.Empty;
    }
    // ================================================================
}



/*
[System.Serializable]
public class Dialogue
{
    [Header("대사를 칠 인물 이름입니다!")]
    public string name;

    [Header("인물이 칠 대사입니다! 줄 당 32글자를 넘지 않게 해주세요.\n\n참고 ) 연출을 위해 커맨드를 몇 개 탑재했습니다.\n<comm> 처럼 입력해 쓰면 됩니다.\n\n<sleep(0.2)> : 0.2초 딜레이를 가집니다.\n<color(red,범인)> : \'범인\'이란 글자가 빨간색으로 출력됩니다.\n ㄴ 현재 사용 가능 색 : red, green. 추가 가능. 문의 바람!")]
    [TextArea]
    public string content;
}
*/