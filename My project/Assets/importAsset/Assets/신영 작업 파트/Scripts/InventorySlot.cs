using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;  // 아이템 아이콘
    public Item item;  // 슬롯에 있는 아이템 
    public int count;
    public Text countUI;

    private void Start()
    {
        countUI = transform.GetChild(1).GetComponent<Text>();
    }

    private void Update()
    {
        if (item != null) 
        {
            if (count == 1) // 개수 1개 이상부터 개수 출력
                countUI.text = null;
            else
                countUI.text = count.ToString();
        }
        else
            countUI.text = null;
    }
    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        this.count = newItem.count;
    }

    public void ClearSlot()
    {
        countUI.text = null;
        count = 0;
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public bool IsEmpty()
    {
        return item == null;
    }
}