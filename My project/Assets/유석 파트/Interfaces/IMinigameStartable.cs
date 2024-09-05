using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMinigamable
{
    public GameObject GetGameObject();
    public Vector3 GetContainerOffset();
    public void RequestToStartMinigame();
    public void OnMyMinigameFinished(string minigameName);
    public void OnMyMinigameFailed(string minigameName);
    public void OnKeyInputSuccessed();
}
