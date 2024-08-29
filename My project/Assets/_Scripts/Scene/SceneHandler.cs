using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Teleport : MonoBehaviour
{
    public string targetSceneName = "SceneB"; // ��ȯ�ϰ��� �ϴ� ���� �̸�
    private PlayerInputHandler playerInputHandler; // PlayerInputHandler ����
    private bool playerIsInTeleportZone = false;
    private FadeManager fadeManager; // FadeManager ����

    private void Start()
    {
        // �÷��̾� ������Ʈ���� PlayerInputHandler�� ������
        playerInputHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputHandler>();

        // �� ���� FadeManager�� ã��
        fadeManager = FindObjectOfType<FadeManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // �÷��̾ �ڷ���Ʈ ���� ���Դ��� Ȯ��
        {
            playerIsInTeleportZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // �÷��̾ �ڷ���Ʈ ���� ������� Ȯ��
        {
            playerIsInTeleportZone = false;
        }
    }

    private void Update()
    {
        // �÷��̾ �ڷ���Ʈ ���� �ְ� interactionInput�� �����Ǹ� ���̵� �ƿ� �� �� ��ȯ
        if (playerIsInTeleportZone && playerInputHandler.InteractionInput)
        {
            fadeManager.FadeToScene(targetSceneName); // ���̵� �ƿ� �� �� ��ȯ ȣ��
        }
    }
}