using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorway : RestaurantComponent, IInteractable
{
    public void Interact(GameObject interactRequester)
    {
        throw new System.NotImplementedException();
    }

    public bool IsShouldHidePlayer()
    {
        return false;
    }
}
