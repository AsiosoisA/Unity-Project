using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class DialogueDataManager : MonoBehaviour
{
    private Dictionary<DialogueKey, string[]> dialogueData;
    private IdDictionary ID_Dictionary;
    public const string objDict = "object";
    public const string questDict = "quest";

    void Start(){
        dialogueData = new Dictionary<DialogueKey, string[]>();
        ID_Dictionary = EventGameManager.Instance.ID_Dictionary;

        if(ID_Dictionary == null) ID_Dictionary = new IdDictionary(); // 거의 그럴 일 없긴 할텐데 아직 초기화 안 됐으면 걍 새로 만들자.

        GenerateData();
    }

    /*
        자료구조 : 

        KEY : 인물 id : 퀘스트 id : 퀘스트 진척도 : 대사 id
        VALUE : 대사 string[]

        형태의 딕셔너리.
    */
    private int talkerId;
    private int questId;

    void GenerateData(){ // 상속받는 객체들은 모두 이 메소드를 Override 할 것.


        InitTalker(GetId(objDict, "Player"));
        /*
            플레이어의 대사

            관여하는 퀘스트 : main
        */
        // 메인 퀘스트 =====================================
        InitQuest(GetId(questDict, "main"));
        dialogueData.Add(new DialogueKey( talkerId , questId, 0, 0),
        new string[]{
            "흠...",
            "이게 맞나....?"
        });
        dialogueData.Add(new DialogueKey( talkerId , questId, 0, 1),
        new string[]{
            "이게.<sleep(0.5)>.<sleep(0.5)>.<sleep(2)> 맞나....?"
        });
        // ================================================






        InitTalker(GetId(objDict, "NPC1"));
        /*
            NPC1 의 대사

            관여하는 퀘스트 : main
        */
        // 메인 퀘스트 =========================================
        InitQuest(GetId(questDict, "main"));
        dialogueData.Add(new DialogueKey( talkerId , questId, 0, 0),
        new string[]{
            "흠...",
            "이게 맞나요....?"
        });
        dialogueData.Add(new DialogueKey( talkerId , questId, 0, 1),
        new string[]{
            "흠.............",
            "전 NPC인데요... <sleep(2)>이게.<sleep(0.5)>.<sleep(0.5)>.<sleep(2)> 맞나요....?"
        });
        // ======================================================
    
    
    
    
    }

    public string[] GetTalk(int talkerId, int questId, long questStat, int sentenceId){
        DialogueKey key = new DialogueKey(talkerId, questId, questStat, sentenceId);
        
        if(dialogueData.ContainsKey(key)){
            return dialogueData[key];
        }
        else throw new ArgumentOutOfRangeException("DialogueDataManager : " + key.ToString() + "에 해당하는 대화는 존재하지 않습니다!");
    }
    // 해당 함수는 Overloading에 의해 talker 의 이름, 퀘스트의 이름을 직접 입력해도 작동함.

    /*
    #region 오버로딩
    // Overloading
    public string[] GetTalk(string talkerName, string questName, long questStat, int sentenceId){
        int talkerId = GetId(objDict, talkerName);
        int questId = GetId(questDict, questName);
        return GetTalk(talkerId, questId, questStat, sentenceId);
    }
    // Overloading
    public string[] GetTalk(string talkerName, int questId, long questStat, int sentenceId){
        int talkerId = GetId(objDict, talkerName);
        return GetTalk(talkerId, questId, questStat, sentenceId);
    }
    // Overloading
    public string[] GetTalk(int talkerid, string questName, long questStat, int sentenceId){
        int questId = GetId(questDict, questName);
        return GetTalk(talkerId, questId, questStat, sentenceId);
    }
    #endregion
    */ // 필요없을지도?

    private void InitTalker(int id){
        talkerId = id;
    }
    private void InitQuest(int id){
        questId = id;
    }

    public int GetId(string dictType, string key){
        if(dictType == objDict){
            return ID_Dictionary.GetId(ID_Dictionary.objects, key);
        }
        else if(dictType == questDict){
            return ID_Dictionary.GetId(ID_Dictionary.quests, key);
        }
        else return -1;
    }

    public string GetName(string dictType, int key){
        if(dictType == objDict){
            return ID_Dictionary.GetName(ID_Dictionary.objectIds, key);
        }
        else if(dictType == questDict){
            return ID_Dictionary.GetName(ID_Dictionary.questIds, key);
        }
        else return null;
    }
}

public struct DialogueKey
{
    long questStat; // 퀘스트 분기값입니다.  <- 메모리 패딩 때문에 앞에 위치시킴.
    /*
        최초 0 시작.

        이후 분기 하나가 추가되면 경우의 수 1 , 2 가 되고

        그 시점에서 또 분기가 생성되면 11 , 12 , 21 , 22 처럼 됨.

        즉 branch = questStat * 10 + branchNumber;

        지금 분기가 어떤 분기에서 뻗어나온건지 알고 싶다면 questStat / 10 하면 됨.
    */
    short talkerId; // 대사를 칠 인물 id 입니다.
    short questId; // 퀘스트 id 입니다.
    short sentenceId; // 대사 id 입니다.
    /*
        총 14바이트의 정보를 담고 있음. string 14글자에 해당하는 Key.

        인물 id : 50 , 퀘스트 id : 100 , 퀘스트의 최초 분기점 , 대화 번호는 80 이라면
        Key : 50_100_0_80 이렇게 됨.

        만약 여기서 분기가 두 개 생겨서
        1 분기는 참
        2 분기는 거짓 이라면

        Key : 50_100_12_80 이렇게 됨.

        여기서 전 분기로 넘어간다면 12 / 10 = 1 이니까 50_100_1_80 으로 넘어가짐.

        대화 번호는 분기를 넘어갔다고 0으로 초기화하지 말 것. 이 대화 번호 자체를 ID 로 생각하고 보는게 흐름을 파악하는 데에 용이함.
    */

    public DialogueKey(int talkerId, int questId, long questStat, int sentenceId){

        if (talkerId < short.MinValue || talkerId > short.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(talkerId), "Talker ID must be within the range of a short.");
        if (questId < short.MinValue || questId > short.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(questId), "Quest ID must be within the range of a short.");
        if (sentenceId < short.MinValue || sentenceId > short.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(sentenceId), "Sentence ID must be within the range of a short.");

        this.talkerId = (short)talkerId; // TODO 설마 그럴 일은 없겠지만 인물 수가 32767개를 넘어간다면 수정해야 함.
        this.questId = (short)questId; // TODO 설마 그럴 일은 없겠지만 퀘스트 수가 32767개를 넘어간다면 수정해야 함.
        this.questStat = questStat; // TODO 퀘스트 분기 조합이 10개를 넘어간다면 수정해야 함.
        this.sentenceId = (short)sentenceId; // TODO 대화 수가 32767개를 넘어간다면 수정해야 함.
    }

    public override bool Equals(object obj)
    {
        if(obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        DialogueKey other = (DialogueKey)obj;

        return 
            talkerId == other.talkerId && 
            questId == other.questId &&
            questStat == other.questStat &&
            sentenceId == other.sentenceId;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(talkerId, questId, questStat, sentenceId);
    }

    public override string ToString()
    {
        return $"{talkerId}_{questId}_{questStat}_{sentenceId}";
    }
}

















