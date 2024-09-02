using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices.WindowsRuntime;

public abstract class SO_MinigameData : ScriptableObject
{
    public string minigameName;
    public abstract Type GetMinigameType();
}
