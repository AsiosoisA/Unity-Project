using Bardent.Weapons;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public WeaponDataSO defaultweapon;
    public int inventorySize = 20; // �κ��丮 ������ ����
    public GameObject slotPrefab;  // ���� UI ������
    public Transform slotParent;   // ���Ե��� ��ġ�� �θ� ��ü (Grid Layout Group�� �ִ� ������Ʈ)
    public CanvasController cvs;
    CanvasGroup cvg;
    WeaponGenerator weaponGenerator;

    public Item testItem; //�׽�Ʈ ��

    public bool isRun = false;

    private List<GameObject> slots = new List<GameObject>();

    private void Start()
    {
        // �κ��丮 ������ �����ϰ� Grid Layout Group�� �߰�
        for (int i = 0; i < inventorySize; i++)
        {
            GameObject slot = Instantiate(slotPrefab, slotParent);
            slots.Add(slot);
        }

        cvs = transform.GetComponentInParent<CanvasController>();
        cvg = GetComponent<CanvasGroup>();
        weaponGenerator = GameObject.Find("Player").transform.GetChild(2).GetComponent<WeaponGenerator>();

        // ����Ʈ ���⸦ �� �հ����� 
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
        // �� ������ ã�� �������� �߰�
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