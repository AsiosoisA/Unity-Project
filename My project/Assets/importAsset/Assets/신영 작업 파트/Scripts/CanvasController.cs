using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Bardent.Weapons;
using UnityEngine.InputSystem;

public class CanvasController : MonoBehaviour
{
    public GraphicRaycaster uiRaycaster; 
    public EventSystem eventSystem;
    GameObject OptionSet;
    CanvasGroup OptionCVG;
    RectTransform OptionRect;
    Inventory inventory;
    Color selectedColor;
    Color unselectedColor;

    GameObject preObject;
    public GameObject selectedItem;
    bool isInventoryOpen = false;


    private void Start()
    {
        OptionSet = transform.GetChild(3).gameObject;
        OptionCVG = OptionSet.GetComponent<CanvasGroup>();
        OptionRect = OptionSet.GetComponent<RectTransform>();
        inventory = transform.GetChild(2).GetComponent<Inventory>();
        selectedColor = new Color(190, 190, 190, 255);
        unselectedColor = new Color(255, 255, 255, 255);
    }

    void Update()
    {
    }//인풋시스템에 맞도록 구현
    public void MouseRightButtonDown(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // 1. UI Raycast 처리
            PointerEventData pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = Mouse.current.position.ReadValue();
            List<RaycastResult> results = new List<RaycastResult>();
            uiRaycaster.Raycast(pointerEventData, results);

            if (results.Count > 0)
            {
                selectedItem = results[0].gameObject;
                // UI 요소에 맞았을 때 처리
                if (selectedItem.tag == "slot")
                {

                    //활성화
                    OptionRect.position = new Vector3(selectedItem.transform.position.x + 120, selectedItem.transform.position.y - 90, 1);
                    OptionCVGEnabler();

                    if (selectedItem.GetComponent<InventorySlot>().item == null)
                        return;
                }
            }
        }
    }

    public void inventoryButtonDown(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isInventoryOpen)
            {
                inventory.InventoryDisabler();
                isInventoryOpen = false;
            }
            else
            {
                inventory.InventoryEnabler();
                isInventoryOpen = true;
            }
        }            
    }

    public void OptionCVGEnabler()
    {
        OptionCVG.alpha = 1;
        OptionCVG.blocksRaycasts = true;
        OptionCVG.interactable = true;
    }

    public void OptionCVGDisabler()
    {
        OptionCVG.alpha = 0;
        OptionCVG.blocksRaycasts = false;
        OptionCVG.interactable = false;
    }
}
