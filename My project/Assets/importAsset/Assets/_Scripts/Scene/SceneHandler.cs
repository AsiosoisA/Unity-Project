using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Teleport : MonoBehaviour
{
    public string targetSceneName = "SceneB"; // 전환하고자 하는 씬의 이름
    private PlayerInputHandler playerInputHandler; // PlayerInputHandler 참조
    private bool playerIsInTeleportZone = false;
    private FadeManager fadeManager; // FadeManager 참조

    private void Start()
    {
        // 플레이어 오브젝트에서 PlayerInputHandler를 가져옴
        playerInputHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputHandler>();

        // 씬 내의 FadeManager를 찾음
        fadeManager = FindObjectOfType<FadeManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 플레이어가 텔레포트 존에 들어왔는지 확인
        {
            playerIsInTeleportZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 플레이어가 텔레포트 존을 벗어났는지 확인
        {
            playerIsInTeleportZone = false;
        }
    }

    private void Update()
    {
        // 플레이어가 텔레포트 존에 있고 interactionInput이 감지되면 페이드 아웃 후 씬 전환
        if (playerIsInTeleportZone && playerInputHandler.InteractionInput)
        {
            fadeManager.FadeToScene(targetSceneName); // 페이드 아웃 및 씬 전환 호출
        }
    }
}