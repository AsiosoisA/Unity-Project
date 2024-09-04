using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KeyMinigame : Minigame
{
    public SO_KeyCustom keyCustom;
    public KeyContainer keyContainer {get; private set;}
    public new SO_KeyMinigame minigameData;
    public Dictionary<string, bool> usingKey = new Dictionary<string, bool>();

    public KeyMinigame(MinigameManager manager) : base(manager)
    {
        this.keyContainer = manager.keyContainer;
    }

    public override SO_MinigameData GetData() => minigameData;

    public virtual void OnKeyInputSucessed(){}
    public virtual void OnKeyLifecycleFinished(){}
    public virtual void OnReadyToGetNextKey(){}
    public virtual void OnKeyPressed(){}
    public virtual void OnKeyUped(){}
    public bool IsThisKeyUsing(string keyCode)
    {
        return usingKey.ContainsKey(keyCode);
    }
    public void SetThisKeyUsing(string keyCode)
    {
        usingKey.Add(keyCode, true);
    }
    public void SetThisKeyNoLongerUsing(string keyCode)
    {
        usingKey.Remove(keyCode);
    }
    protected void InitKeyCodeOfKey(KeyInstance keyInstance, string keyCode)
    {
        KeyCodeComponent keyCodeComponent = keyInstance.keyCodeComponent;

        if(keyCodeComponent == null) Debug.LogError("키코드 컴포넌트를 제대로 할당하세요!");

        keyCodeComponent.ChangeSpriteLibrary(manager.pool.keycodeComponentLooks[keyCode]);
    }
    
}
