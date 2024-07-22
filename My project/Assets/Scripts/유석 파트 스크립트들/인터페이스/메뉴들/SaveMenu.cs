using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveMenu : SaveLoadSlotMenu
{

    //saveLoadSlots 라는 슬릇 리스트 맴버가 있음.

    public override void ExtraStartLogic() // override 하기만 해도 알아서 잘 돌아감!
    {
        foreach(SaveLoadSlot slot in saveLoadSlots)
        {
            Button button = slot.GetComponent<Button>();
            button.onClick.AddListener(slot.OnSlotClickForSave);
        }
    }
    
}
