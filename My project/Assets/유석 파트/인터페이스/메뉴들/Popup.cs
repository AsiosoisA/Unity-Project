using System.Collections;
using System.Collections.Generic;
using UnityEditor.ProjectWindowCallback;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public Text popupContent;
    private CanvasGroup canvasGroup;
    bool isCustom;

    public void SetContent(string content){
        popupContent.text = content;
    }

    public void Begin(bool isCustom){
        Debug.Log("팝업 생성 시작!");
        this.isCustom = isCustom;

        canvasGroup = GetComponent<CanvasGroup>();

        StartCoroutine(PadeInAndOut());
    }

    private float fadeDuration = 0.5f;
    IEnumerator PadeInAndOut(){
        canvasGroup.alpha = 0f;
        gameObject.SetActive(true);

        while(canvasGroup.alpha < 1f){
            canvasGroup.alpha += Time.deltaTime / fadeDuration;
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        while(canvasGroup.alpha > 0f){
            canvasGroup.alpha -= Time.deltaTime / fadeDuration;
            yield return null;
        }

        End();
    }

    void End(){
        if(!isCustom) Destroy(gameObject);
        else gameObject.SetActive(false);
    }
}
