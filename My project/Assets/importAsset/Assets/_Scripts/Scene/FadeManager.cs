using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    public Image fadeImage; // 페이드 이미지를 연결
    public float fadeDuration = 1f; // 페이드 지속 시간

    private void Start()
    {
        // 씬이 시작될 때 페이드 인
        StartCoroutine(FadeIn());
    }

    public void FadeToScene(string sceneName)
    {
        // 지정한 씬으로 페이드 아웃 후 씬 전환
        StartCoroutine(FadeOut(sceneName));
    }

    private IEnumerator FadeIn()
    {
        // 페이드 인 효과
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0f, 0f, 0f, 1f);

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeImage.color = new Color(0f, 0f, 0f, 1f - Mathf.Clamp01(elapsedTime / fadeDuration));
            yield return null;
        }

        fadeImage.color = new Color(0f, 0f, 0f, 0f);
        fadeImage.gameObject.SetActive(false);
    }

    private IEnumerator FadeOut(string sceneName)
    {
        // 페이드 아웃 효과
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0f, 0f, 0f, 0f);

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeImage.color = new Color(0f, 0f, 0f, Mathf.Clamp01(elapsedTime / fadeDuration));
            yield return null;
        }

        // 씬 전환
        SceneManager.LoadScene(sceneName);
    }
}