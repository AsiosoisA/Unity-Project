using Bardent.Weapons;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public WeaponDataSO defaultweapon;
    public int inventorySize = 20; // 인벤토리 슬롯의 개수
    public GameObject slotPrefab;  // 슬롯 UI 프리팹
    public Transform slotParent;   // 슬롯들이 배치될 부모 객체 (Grid Layout Group이 있는 오브젝트)
    public CanvasController cvs;
    CanvasGroup cvg;
    WeaponGenerator weaponGenerator;

    public Item testItem; //테스트 용

    public bool isRun = false;

    private List<GameObject> slots = new List<GameObject>();

    private void Start()
    {
        // 인벤토리 슬롯을 생성하고 Grid Layout Group에 추가
        for (int i = 0; i < inventorySize; i++)
        {
            GameObject slot = Instantiate(slotPrefab, slotParent);
            slots.Add(slot);
        }

        cvs = transform.GetComponentInParent<CanvasController>();
        cvg = GetComponent<CanvasGroup>();
        weaponGenerator = GameObject.Find("Player").transform.GetChild(2).GetComponent<WeaponGenerator>();

        // 디폴트 무기를 한 손검으로 
        weaponGenerator.OnTest(defaultweapon);
    }

    private void Update()
    {
        if (isRun)
        {
            AddItem(testItem);
            isRun = false;
        }
    }

    public void AddItem(Item item)
    {
        // 빈 슬롯을 찾아 아이템을 추가
        foreach (var slot in slots)
        {
            InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
            if (inventorySlot.IsEmpty())
            {
                inventorySlot.AddItem(item);
                break;
            }
        }
    }

    public void throwItem()
    {
        InventorySlot tmp = cvs.selectedItem.GetComponent<InventorySlot>();
        tmp.ClearSlot();
        cvs.OptionCVGDisabler();
    }

    public void equipItem()
    {
        InventorySlot tmp = cvs.selectedItem.GetComponent<InventorySlot>();
        if (tmp.item.itemType == Item.ItemType.Weapon)
        {
            weaponGenerator.OnTest(tmp.item.weaponData);
            cvs.OptionCVGDisabler();
        }
    }

    public void cancelButton()
    {
        cvs.OptionCVGDisabler();
    }

    public void InventoryEnabler()
    {
        cvg.alpha = 1;
        cvg.blocksRaycasts = true;
        cvg.interactable = true;
    }

    public void InventoryDisabler()
    {
        cvg.alpha = 0;
        cvg.blocksRaycasts = false;
        cvg.interactable = false;
    }
}