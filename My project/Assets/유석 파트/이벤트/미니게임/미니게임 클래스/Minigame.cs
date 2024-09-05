using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[System.Serializable]
public class Minigame
{
    public MinigameManager manager;
    public GameObject requester;
    public bool isMinigaming;
    public bool isClearMinigame = false;
    public SO_MinigameData minigameData;
    public Minigame(MinigameManager manager)
    {
        this.manager = manager;
    }
    public virtual SO_MinigameData GetData() => minigameData;

    public virtual void StartMinigame(GameObject requester, Vector3 offset)
    {
        isMinigaming = true;
        this.requester = requester;
    }

    public virtual void OnMinigameFinished()
    {
        isMinigaming = false;
    }
    public virtual void OnMinigameFailed()
    {
        isMinigaming = false;
    }
}
