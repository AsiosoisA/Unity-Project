using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AlertManager : MonoBehaviour
{
    // ===========================    요 객체는 두고두고 어디에서든 자주 호출할 수 있을 것 같아 싱글톤 패턴으로 만들겠음. ==============================
    private static AlertManager instance;
    public static AlertManager Instance
    {
        get{
            if(instance == null)
            {
                instance = FindObjectOfType<AlertManager>();
                
                if(instance == null){
                    instance = new GameObject().AddComponent<AlertManager>();                    
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<AlertManager>();
        if(objs.Length != 1){
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    // ===========================================================================================================================================

    



    public GameObject alertPrefab;

    public void MakeAlert(string title, string content, UnityAction posListener, UnityAction negListener){
        Alert newAlert = Instantiate(alertPrefab, transform).GetComponent<Alert>();
        newAlert.SetTitle(title);
        newAlert.SetContent(content);
        newAlert.AddPosBtnListener(posListener);
        newAlert.AddNegBtnListener(negListener);

        newAlert.Begin(false);
    }

    public void MakeAlert(Alert customAlert, UnityAction posListener, UnityAction negListener){
        customAlert.AddPosBtnListener(posListener);
        customAlert.AddNegBtnListener(negListener);

        customAlert.Begin(true);
    }
}
