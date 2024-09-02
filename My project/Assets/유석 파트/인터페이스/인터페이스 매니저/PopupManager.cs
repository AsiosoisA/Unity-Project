using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    // ===========================    요 객체는 두고두고 어디에서든 자주 호출할 수 있을 것 같아 싱글톤 패턴으로 만들겠음. ==============================
    // 아니, 싱글톤 패턴은 상속 안 되는거 실화인가요?
    private static PopupManager instance;
    public static PopupManager Instance
    {
        get{
            if(instance == null)
            {
                instance = FindObjectOfType<PopupManager>();
                
                if(instance == null){
                    instance = new GameObject().AddComponent<PopupManager>();                    
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<PopupManager>();
        if(objs.Length != 1){
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    // ===========================================================================================================================================


    public GameObject popupPrefab;
    public Transform whereToPut;

    public void MakePopup(string content){
        Popup newPopup = Instantiate(popupPrefab, whereToPut).GetComponent<Popup>();
        newPopup.SetContent(content);

        newPopup.Begin(false);
    }

    public void MakePopup(Popup customPopup){
        customPopup.Begin(true);
    }
}
