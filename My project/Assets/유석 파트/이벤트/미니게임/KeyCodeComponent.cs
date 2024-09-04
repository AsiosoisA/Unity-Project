using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class KeyCodeComponent : KeyInstanceComponent
{
    private SpriteLibrary library;
    //private SpriteResolver resolver;

    protected override void Awake()
    {
        base.Awake();
    
        library = GetComponent<SpriteLibrary>();
        //resolver = GetComponent<SpriteResolver>();
    }

    protected override void SetMeToMyKeyInstance()
    {
        myKeyInstance.SetKeyCodeComponent(this);
    }

    public void ChangeSpriteLibrary(SpriteLibraryAsset asset)
    {
        if(library != null && asset != null)
        {
            library.spriteLibraryAsset = asset;
            library.RefreshSpriteResolvers();
        }
        else
        {
            Debug.LogError("인자를 제대로 넣어주세요!");
        }
    }
}
