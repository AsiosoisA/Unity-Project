using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class IdDictionary
{
    
    public Dictionary<string, int> objects;
    public Dictionary<string, int> quests;
    
    public Dictionary<int, string> objectIds;
    public Dictionary<int, string> questIds;
    private int itemIndex;

    public IdDictionary(){
        objects = new Dictionary<string, int>();
        quests = new Dictionary<string, int>();

        objectIds = new Dictionary<int, string>();
        questIds = new Dictionary<int, string>();

        // ===========================  캐릭터 이름, id 넣는 곳 ==============================
        InitIndex(1000); // 시작 아이디는 1000으로 한다.

        Add(objects, objectIds, "Player");
        Add(objects, objectIds, "NPC1");
        Add(objects, objectIds, "NPC2");

        // ==================================================================================
    
    
        






        // ===========================  퀘스트 이름, id 넣는 곳 ===============================
        InitIndex(0); // 시작 아이디는 0으로 한다.

        Add(quests, questIds, "main");
        Add(quests, questIds, "보어 10마리 퇴치하기");
        // ===================================================================================

    }
    private void InitIndex(int startIdx){
        itemIndex = startIdx;
    }

    private void Add(Dictionary<string, int> dict, Dictionary<int,string> idDict, string key){
        dict.Add(key, itemIndex);
        idDict.Add(itemIndex, key);
        itemIndex++;
    }

    
    public int GetId(Dictionary<string, int> dict , string key){
        if(dict.ContainsKey(key)){
            return dict[key];
        }
        else throw new System.ArgumentOutOfRangeException("IdDictionary : key 값 " + key + "에 해당하는 아이템이 딕셔너리에 존재하지 않습니다.");
    }

    public string GetName(Dictionary<int, string> dict, int key){
        if(dict.ContainsKey(key)){
            return dict[key];
        }
        else throw new System.ArgumentOutOfRangeException("IdDictionary : key 값 " + key + "에 해당하는 아이템이 딕셔너리에 존재하지 않습니다.");
    }
}