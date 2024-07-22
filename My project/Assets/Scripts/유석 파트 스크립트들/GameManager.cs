using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameManager : MonoBehaviour
{
    private static GameManager instance; // 밖에서 참조할 때는 Instance 로 참조함.
    #region 싱글톤 패턴 구현
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                var obj = FindObjectOfType<GameManager>();
                if(obj != null)
                {
                    instance = obj;
                }
                else
                {
                    instance = new GameObject().AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<GameManager>();
        if(objs.Length != 1){
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);


        Init();
    }
    #endregion

    public IdDictionary ID_Dictionary;

    public DialogueDataManager dialogueDataManager;

    public DialogueSystem dialogueSystem;


    // Start is called before the first frame update
    void Init()
    {
        if(ID_Dictionary == null) ID_Dictionary = new IdDictionary();

        if(dialogueDataManager == null) throw new System.ArgumentException("GameManager : DialogueDataManager를 할당해주세요!");

        if(dialogueSystem == null) throw new System.ArgumentException("GameManager : DialogueSystem을 할당해주세요!");
    }

    // Update is called once per frame
    void Start()
    {
        Debug.Log("GameManager : Start");
        dialogueSystem.TalkStart("NPC1", "main", 0, 1);
    }
}
