using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{
    #region Variables & Components
    public CinemachineVirtualCamera playerCamera;
    public Player player;

    private float initialCameraSize;
    private float maxZoomIn;
    private float zoomInSpeed = 2f;
    private float zoomOutSpeed = 10f;
    private float maxZoomInFactor = 0.8f;

    private Vector3 initialCameraPosition;
    private float shakeMagnitude = 0.2f;
    private float shakeFrequency = 20f; // �ʴ� ���� Ƚ��
    private float shakeElapsed = 0f;
    private bool isShaking = false;
    private Transform originalFollowTarget; // ���� Follow ����� �����մϴ�
    #endregion

    #region Unity Callback Functions
    private void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();
        }
        player = GameObject.Find("Player").GetComponent<Player>();
        initialCameraSize = playerCamera.m_Lens.OrthographicSize;
        maxZoomIn = initialCameraSize * maxZoomInFactor;
        initialCameraPosition = playerCamera.transform.position;

        originalFollowTarget = playerCamera.Follow;
    }
    private void Update()
    {
        if (isShaking)
        {
            ShakeCamera();
        }
    }
    #endregion

    #region Camera Functions
    public bool CheckIfInitZoom()
    {
        return initialCameraSize == playerCamera.m_Lens.OrthographicSize;
    }

    public void ApplyCameraZoom(float chargeTime, float maxHoldTime, float holdThreshold)
    {
        float zoomFactor = Mathf.Clamp01(chargeTime / maxHoldTime);
        float newCameraSize = Mathf.Lerp(initialCameraSize, maxZoomIn, zoomFactor);
        playerCamera.m_Lens.OrthographicSize = Mathf.Lerp(playerCamera.m_Lens.OrthographicSize, newCameraSize, Time.deltaTime * zoomInSpeed);

        if (chargeTime > holdThreshold)
        {
            isShaking = true;

            playerCamera.Follow = null;

            playerCamera.transform.position = player.transform.position;
            initialCameraPosition = player.transform.position;
        }
    }

    public void ResetCameraZoom()
    {
        playerCamera.m_Lens.OrthographicSize = Mathf.Lerp(playerCamera.m_Lens.OrthographicSize, initialCameraSize, Time.deltaTime * zoomOutSpeed);

        if (Mathf.Abs(playerCamera.m_Lens.OrthographicSize - initialCameraSize) < 0.01f)
        {
            playerCamera.m_Lens.OrthographicSize = initialCameraSize;
            isShaking = false; // ��鸲 ����

        }
    }

    public void ResetFollowingObject() => playerCamera.Follow = originalFollowTarget;

    private void ShakeCamera()
    {
        shakeElapsed += Time.deltaTime;
        float xOffset = Mathf.Sin(shakeElapsed * shakeFrequency) * shakeMagnitude;
        float yOffset = Mathf.Cos(shakeElapsed * shakeFrequency) * shakeMagnitude;

        playerCamera.transform.position = new Vector3(initialCameraPosition.x + xOffset, initialCameraPosition.y + yOffset, -10f);
    }
    #endregion
}