using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    [System.Serializable]
    public class EnemyLootTable
    {
        public string enemyName; // ���� �̸�
        public string itemName; // �ش� ���� ����� ������ �̸�
    }

    public List<EnemyLootTable> enemyLootTables; // ��ü ���� ������ ��� ���̺� ����Ʈ
    private Dictionary<string, string> enemyLootDictionary; // �� �̸��� Ű�� ����Ͽ� �������� ã�� �� �ִ� Dictionary

    void Awake()
    {
        enemyLootDictionary = new Dictionary<string, string>();
        foreach (var entry in enemyLootTables)
        {
            if (!enemyLootDictionary.ContainsKey(entry.enemyName))
            {
                enemyLootDictionary.Add(entry.enemyName, entry.itemName);
            }
        }
    }

    public void HandleLoot(string lootingObject)
    {
        if (enemyLootDictionary.TryGetValue(lootingObject, out string itemName))
        {
            //Inventory.AddItem(itemName)
        }
        else
        {
            Debug.LogWarning("���� ���� �������� ã�� �� �����ϴ�: " + lootingObject);
        }
    }
}