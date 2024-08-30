using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SO_ACItemDictionary : ScriptableObject
{
    //public Dictionary<string, List<SO_ACItem>> acItemDictionary;
    public List<string> dictionaryKeys = new List<string>();
    public List<SO_ACItems> dictionaryItems = new List<SO_ACItems>();    
}

[Serializable]
public class SO_ACItems
{
    public List<SO_ACItem> items = new List<SO_ACItem>(); 
}
