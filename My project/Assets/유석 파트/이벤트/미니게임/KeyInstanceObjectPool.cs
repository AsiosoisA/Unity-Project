using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class KeyInstanceObjectPool : ObjectPool
{
    [SerializeField] private List<KeyCodeLookPair> keycodeComponentLooksList = new List<KeyCodeLookPair>();
    public Dictionary<string, SpriteLibraryAsset> keycodeComponentLooks = new Dictionary<string, SpriteLibraryAsset>();

    protected override void Awake()
    {
        base.Awake();

        foreach(KeyCodeLookPair pair in keycodeComponentLooksList)
        {
            keycodeComponentLooks.Add(pair.keyCode, pair.asset);
        }
    }
}

[System.Serializable]
public class KeyCodeLookPair
{
    public string keyCode;
    public SpriteLibraryAsset asset;
}
