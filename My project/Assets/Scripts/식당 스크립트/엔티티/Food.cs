using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Food : MonoBehaviour
{
    public string foodName;
    public Sprite foodImage;
    public int tasteGrade;
    public int price;

    public List<RecipePair> recipe;
    
    // 요리사에 대한 정보도 여기에 추가할 것.

    public abstract void cook();
    public abstract void unlockCheck();
}
