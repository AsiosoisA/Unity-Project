using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    public Image fadeImage; // ���̵� �̹����� ����
    public float fadeDuration = 1f; // ���̵� ���� �ð�

    private void Start()
    {
        // ���� ���۵� �� ���̵� ��
        StartCoroutine(FadeIn());
    }

    public void FadeToScene(string sceneName)
    {
        // ������ ������ ���̵� �ƿ� �� �� ��ȯ
        StartCoroutine(FadeOut(sceneName));
    }

    private IEnumerator FadeIn()
    {
        // ���̵� �� ȿ��
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
        // ���̵� �ƿ� ȿ��
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0f, 0f, 0f, 0f);

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeImage.color = new Color(0f, 0f, 0f, Mathf.Clamp01(elapsedTime / fadeDuration));
            yield return null;
        }

        // �� ��ȯ
        SceneManager.LoadScene(sceneName);
    }
}