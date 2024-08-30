using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Alert : MonoBehaviour
{
    public GameObject display;
    public Text alertTitle;
    public Text alertContent;
    public Button positiveButton;
    public Button negativeButton;

    public Text positiveButtonText;
    public Text negativeButtonText;

    private UnityAction posListener;
    private UnityAction negListener;

    bool isCustom;
    
    public void Begin(bool isCustom){
        this.isCustom = isCustom;

        gameObject.SetActive(true);
    }


    public void AddPosBtnListener(UnityAction listener){
        posListener = listener;

        positiveButton.onClick.AddListener(OnClickPosBtn);
    }
    public void AddNegBtnListener(UnityAction listener){
        negListener = listener;

        negativeButton.onClick.AddListener(OnClickNegBtn);
    }
    public void SetTitle(string title){
        alertTitle.text = title;
    }
    public void SetContent(string content){
        alertContent.text = content;
    }

    public void OnClickPosBtn(){
        if(posListener != null) posListener.Invoke();

        End();
    }

    public void OnClickNegBtn(){
        if(negListener != null) negListener.Invoke();

        End();
    }

    public void SetPositiveButtonText(string text)
    {
        positiveButtonText.text = text;
    }
    public void SetNegativeButtonText(string text)
    {
        negativeButtonText.text = text;
    }

    private void End(){
        if(!isCustom) Destroy(gameObject);
        else gameObject.SetActive(false);
    }
}