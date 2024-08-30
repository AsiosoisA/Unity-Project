using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SaveLoadSlotMenu : MonoBehaviour
{
    public List<SaveLoadSlot> saveLoadSlots;

    void Start(){

        int num = 1;
        foreach(SaveLoadSlot slot in saveLoadSlots){
            slot.slotNumber = num++;
            slot.LoadData();
        }



        ExtraStartLogic();
    }

    public abstract void ExtraStartLogic();


}
