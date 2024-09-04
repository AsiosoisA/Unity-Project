using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GuageBar : MonoBehaviour
{
    public Slider guageBar;

    public float yOffset;
    private Vector3 offset;
    private Vector3 originalOffset;


    public bool isProcessing;
    public float increaseSpeed;
    public float decreaseSpeed;

    void Awake() // 왠진 몰라도 Active 된 직후가 돼야 겨우 발동함
    {
        guageBar = GetComponent<Slider>();

        guageBar.value = 0;

        offset = new Vector3(0, yOffset, 0);
        originalOffset = offset;

        //gameObject.SetActive(false);
    }
    void Update()
    {
        processingCheck();
    }

    private void processingCheck()
    {
        if(isProcessing)
        {
            if(guageBar.value + Time.deltaTime * increaseSpeed < 100) guageBar.value += Time.deltaTime * increaseSpeed;
            else guageBar.value = 100;
        }
        else
        {
            if(guageBar.value - Time.deltaTime * decreaseSpeed > 0) guageBar.value -= Time.deltaTime * decreaseSpeed;
            else guageBar.value = 0;
        }
    }

    public void Init(Transform target, float extraOffset)
    {
        gameObject.SetActive(true);
        offset += new Vector3(0, extraOffset, 0);
        SetPosition(target);
    }

    public void SetPosition(Transform target)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(target.position + offset);
        transform.position = screenPosition;
    }

    public void StopGuage()
    {
        increaseSpeed = 0;
        decreaseSpeed = 0;
    }

    public void IncreaseGuage(float speed)
    {
        if(!isProcessing) isProcessing = true;
        increaseSpeed = speed;
    }

    public void DecreaseGuage(float speed)
    {
        if(isProcessing) isProcessing = false;
        decreaseSpeed = speed;
    }

    public void DecreaseImmediately()
    {
        if(isProcessing) isProcessing = false;
        guageBar.value = 0;
        decreaseSpeed = 0.1f;
    }

    public void Release()
    {
        isProcessing = false;
        offset = originalOffset;
        guageBar.value = 0;
        gameObject.SetActive(false);
    }
}
