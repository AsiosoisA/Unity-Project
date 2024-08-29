using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    [System.Serializable]
    public class EnemyLootTable
    {
        public string enemyName; // 적의 이름
        public string itemName; // 해당 적이 드랍할 아이템 이름
    }

    public List<EnemyLootTable> enemyLootTables; // 전체 적의 아이템 드랍 테이블 리스트
    private Dictionary<string, string> enemyLootDictionary; // 적 이름을 키로 사용하여 아이템을 찾을 수 있는 Dictionary

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
            Debug.LogWarning("적에 대한 아이템을 찾을 수 없습니다: " + lootingObject);
        }
    }
}
