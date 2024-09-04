using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EffectCore : MonoBehaviour
{

    /*
        부모 트랜스폼에 효과를 부여하거나 부모 트랜스폼의 현재 효과 상태를 추적하는 클래스.
    */

    private Transform target;

    #region Magnitude 로직을 위한 값
        private float magnitudeDuration = 1.0f; // 진동 시간
        private float magnitudeElapsedTime = 0.0f; // 경과 시간
        private float magnitude = 0.1f; // 진동 세기
        private Vector3 originalPosition; // 원래 위치
    #endregion

    #region Fade In , Fade Out 로직을 위한 값
        private Renderer targetRenderer;
        private float fadeInOutDuration = 1.0f;
        private float fadeInOutElapsedTime = 0.0f;
        private Color startColor;
        private Color originalColor;
    #endregion

    #region ChangeScale 로직을 위한 값
        private float changeScaleElapsedTime = 0.0f;
        private float changeScaleDuration = 1.0f;
        private Vector3 initialScale;
        private Vector3 goalScale;
        private bool isScaleChanged = false;
        private Vector3 originalScale;
    #endregion

    #region 부모 객체의 효과값 추적을 위한 불리언값
    public bool isMagnituding {get; private set;}
    public bool isFadeIning {get; private set;}
    public bool isFadeOuting {get; private set;}
    public bool isChangingScale {get; private set;}


    public bool isEffectHandling {get; private set;}

    #endregion

    void Awake()
    {
        target = transform.parent.transform;
        targetRenderer = target.GetComponent<Renderer>();

        Init();
    }

    void Update()
    {
        Magnitude_Update();
        Fade_Update();
        ChangeScale_Update();
        isEffectHandlingCheck();
    }

    public void Init()
    {
        if(isEffectHandling)
        {
            Debug.Log("Init 호출 시점에 이미 어떤 효과를 처리중입니다!!!");
            Debug.Log("매그니튜드 : " + isMagnituding + ", 페이드 : " + isFadeIning + "," + isFadeOuting + ", 크기조절 : " + isChangingScale);
        }




        isMagnituding = false;
        isFadeIning = false;
        isFadeOuting = false;
        isChangingScale = false;  
    }

    public void isEffectHandlingCheck()
    {
        if(!isMagnituding && !isFadeIning && !isFadeOuting && isChangingScale) isEffectHandling = false;
        else isEffectHandling = true;
    }

    #region Magnitude

       public void Magnitude(float seconds)
       {
            magnitudeDuration = seconds;

            Magnitude();
       }

       public void Magnitude(float magnitudePower, float seconds)
       {
            magnitude = magnitudePower;
            Magnitude(seconds);
       }

        private void Magnitude()
        {
            if(isMagnituding)
            {
                Magnitude_ResetStatus(); // 얜 그냥 잘 작동해서 냅둬도 됨.
            }
            originalPosition = target.localPosition;
            isMagnituding = true;
        }

        private void Magnitude_Update()
        {
            if(isMagnituding)
            {
                if(magnitudeElapsedTime < magnitudeDuration)
                {
                    magnitudeElapsedTime += Time.deltaTime;

                    float x = Random.Range(-1f, 1f) * magnitude;
                    float y = Random.Range(-1f, 1f) * magnitude;

                    target.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

                    if(magnitudeElapsedTime >= magnitudeDuration)
                    {
                        Magnitude_ResetStatus();
                    }
                }
                else
                {
                    Magnitude_ResetStatus();
                }
            }
        }
        private void Magnitude_ResetStatus()
        {
            target.localPosition = originalPosition;
            magnitudeElapsedTime = 0.0f;
            isMagnituding = false;
        }

    #endregion

    #region FadeInOut
        public void FadeIn(float seconds)
        {
            Fade("in", seconds);
        }

        public void FadeOut(float seconds)
        {
            Fade("out", seconds);
        }

        private void Fade(string type , float seconds)
        {
            fadeInOutDuration = seconds;
            fadeInOutElapsedTime = 0f;

            if(isFadeIning || isFadeOuting) // 엥? 페이드중인데 페이드가 호출되는 경우가 있나?
            {
                isFadeIning = false;
                isFadeOuting = false; // 상태 초기화
                
                // 이 시점에서의 알파값을 일단 확인.
                startColor = targetRenderer.material.color;

                //fadeIn일 때 alpha 는 원래 elapsetime / fadeduration 임. 즉, elapsetime = alpha * fadeDuration.
                if(type == "in")
                {
                    fadeInOutElapsedTime = startColor.a * fadeInOutDuration;
                }
                //fadeOut 일 때는 alpha 는 원래 1 - fadeInOutElapsetime / fadeInOutDuration 임. 즉, ( 1 - a ) * fadeDuration = elapsetime
                else 
                {
                    fadeInOutElapsedTime = ( 1 - startColor.a) * fadeInOutDuration;
                }
            }

            originalColor = targetRenderer.material.color;
            startColor = targetRenderer.material.color;
            if(type == "in") startColor.a = 0.0f;
            else startColor.a = 1.0f;
            targetRenderer.material.color = startColor;

            if(type == "in") isFadeIning = true;
            else isFadeOuting = true;
        }

        private void Fade_Update()
        {
            if(isFadeIning ^ isFadeOuting)
            {
                if(fadeInOutElapsedTime < fadeInOutDuration)
                {
                    fadeInOutElapsedTime += Time.deltaTime;

                    float alpha;
                    if(isFadeIning) alpha = Mathf.Clamp01(fadeInOutElapsedTime / fadeInOutDuration);
                    else alpha = Mathf.Clamp01(1 - fadeInOutElapsedTime / fadeInOutDuration);

                    Color newColor = new Color(startColor.r, startColor.g, startColor.b, alpha);
                    targetRenderer.material.color = newColor;

                    if(fadeInOutElapsedTime >= fadeInOutDuration)
                    {
                        if(isFadeIning) newColor.a = 1.0f;
                        else newColor.a = 0.0f;
                        
                        targetRenderer.material.color = newColor;

                        if(isFadeIning) isFadeIning = false;
                        else isFadeOuting = false;
                    }
                }
                else
                {
                    Color newColor = new Color(startColor.r, startColor.g, startColor.b, startColor.a);

                    if(isFadeIning) newColor.a = 1.0f;
                    else newColor.a = 0.0f;
                    
                    targetRenderer.material.color = newColor;

                    if(isFadeIning) isFadeIning = false;
                    else isFadeOuting = false;
                }
            }
        }

        private void Fade_ResetStatus()
        {
            targetRenderer.material.color = originalColor;
            fadeInOutElapsedTime = 0.0f;
            isFadeIning = false;
            isFadeOuting = false;
        }
    #endregion

    #region ChangeScale
    /*
    public void ChangeScale(float multiplier, float changeScaleDuration) // 버그투성이. 쓸 일이 있으면 그 때 고쳐라!
    {
        if(isChangingScale)
        {
                Debug.LogError("좀 기다리세요! 아직 크기 바꾸는 중입니다.... 엥? 절대 호출되면 안 될텐데?");
                return;
        }

        if(changeScaleDuration == 0)
        {
            target.localScale = new Vector3(originalScale.x * multiplier, originalScale.y * multiplier , originalScale.z);
            return;
        }

        if(!isScaleChanged) originalScale = target.localScale;
        initialScale = target.localScale;
        goalScale = new Vector3(originalScale.x * multiplier, originalScale.y * multiplier , originalScale.z);
        this.changeScaleDuration = changeScaleDuration;
        isScaleChanged = true;
        isChangingScale = true;
    }
    */

    public void ChangeScaleConst(float scaleValue, float changeScaleDuration)
    {
        if(changeScaleDuration == 0)
        {
            target.localScale = new Vector3(scaleValue, scaleValue , originalScale.z);
            return;
        } // 수행시간이 0초라면 그냥 바로 크기를 조절해버리자!

        if(isChangingScale)
        {
            isChangingScale = false; // 일단 상태 정지.

            // 어떤 목표 크기를 향해서 커지거나 작아지고 있을거임.
            // 그 목표 크기에서 시작한다고 가정, elapseTime 을 설정해서 자연스럽게 줄어들도록 만들거임!!!

            float progress = ( target.localScale.x - initialScale.x ) / ( goalScale.x - initialScale.x ) * 100f;

            // progress = elapse / duration 이니까 elapse = progress * duration
            changeScaleElapsedTime = progress * changeScaleDuration;
        }

        if(!isScaleChanged) originalScale = target.localScale;
        initialScale = target.localScale;
        goalScale = new Vector3(scaleValue, scaleValue , originalScale.z);
        this.changeScaleDuration = changeScaleDuration;
        isScaleChanged = true;
        isChangingScale = true;
    }

    private void ChangeScale_Update()
    {
        if(isChangingScale)
        {
            if(changeScaleElapsedTime < changeScaleDuration)
            {
                changeScaleElapsedTime += Time.deltaTime;
                float progress = Mathf.Clamp01(changeScaleElapsedTime / changeScaleDuration);
                target.localScale = Vector3.Lerp(initialScale, goalScale, progress);

                if(changeScaleElapsedTime >= changeScaleDuration)
                {
                    isChangingScale = false;
                    target.localScale = goalScale;
                    changeScaleElapsedTime = 0f;
                }
            }
            else
            {
                isChangingScale = false;
                target.localScale = goalScale;
                changeScaleElapsedTime = 0f;
            }
        }
    }

    private void ChangeScale_ResetStatus()
    {
        target.localScale = originalScale;
        changeScaleElapsedTime = 0.0f;
        isChangingScale = false;
        isScaleChanged = false;
    }
    #endregion
}
