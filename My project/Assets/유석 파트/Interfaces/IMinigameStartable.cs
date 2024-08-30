using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMinigamable
{
    public GameObject GetGameObject();
    public Vector3 GetOffset();
    public void RequestToStartMinigame();
    public void OnMyMinigameFinished(string minigameName);

    public void OnKeyInputSuccessed();
}
