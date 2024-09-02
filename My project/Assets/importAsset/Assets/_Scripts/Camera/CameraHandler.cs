using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{
    #region Variables & Components
    public CinemachineVirtualCamera playerCamera;
    public Player player;

    private float initialFieldOfView;
    private float maxZoomIn;
    private float zoomInSpeed = 2f;
    private float zoomOutSpeed = 10f;
    private float maxZoomInFactor = 0.8f;

    private Vector3 initialCameraPosition;
    private float shakeMagnitude = 0.2f;
    private float shakeFrequency = 20f;
    private float shakeElapsed = 0f;
    private bool isShaking = false;
    private Transform originalFollowTarget;
    #endregion

    #region Unity Callback Functions
    private void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();
        }
        player = GameObject.Find("Player").GetComponent<Player>();
        initialFieldOfView = playerCamera.m_Lens.FieldOfView;
        maxZoomIn = initialFieldOfView * maxZoomInFactor;
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
        return initialFieldOfView == playerCamera.m_Lens.FieldOfView;
    }

    public void ApplyCameraZoom(float chargeTime, float maxHoldTime, float holdThreshold)
    {
        float zoomFactor = Mathf.Clamp01(chargeTime / maxHoldTime);
        float newFieldOfView = Mathf.Lerp(initialFieldOfView, maxZoomIn, zoomFactor);
        playerCamera.m_Lens.FieldOfView = Mathf.Lerp(playerCamera.m_Lens.FieldOfView, newFieldOfView, Time.deltaTime * zoomInSpeed);

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
        playerCamera.m_Lens.FieldOfView = Mathf.Lerp(playerCamera.m_Lens.FieldOfView, initialFieldOfView, Time.deltaTime * zoomOutSpeed);

        if (Mathf.Abs(playerCamera.m_Lens.FieldOfView - initialFieldOfView) < 0.01f)
        {
            playerCamera.m_Lens.FieldOfView = initialFieldOfView;
            isShaking = false;
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
