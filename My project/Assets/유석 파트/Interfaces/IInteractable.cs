using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractableStructure
{
    void Interact(PlayerInteractState interactState, Player requester);

    bool IsShouldHidePlayer();

    void OnInteractFinished();
}
