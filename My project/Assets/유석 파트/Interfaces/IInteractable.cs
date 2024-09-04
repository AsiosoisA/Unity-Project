using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractableStructure
{
    void Interact(GameObject interactRequester);

    bool IsShouldHidePlayer();
}
