using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorway : RestaurantComponent, IInteractableStructure
{
    public void Interact(GameObject interactRequester)
    {
        if(!restaurant.isOpened)
        {
            AlertManager.Instance.MakeAlert(
            "나가기",
            "식당 파트를 종료하고 RPG 세계로 돌아갈까요?",
            GetOut,
            null
            );
        }
    }

    public void GetOut()
    {
        Debug.Log("나감! 여기서 씬 전환하면 됨.");
    }

    public bool IsShouldHidePlayer()
    {
        return false;
    }
}
