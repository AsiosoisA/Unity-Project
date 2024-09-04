using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : FoodStuff
{
    public enum PrepMethod
    {
        Slice,
        Chop
    }

    public static Dictionary<int, string> prefixDictionary = new Dictionary<int, string>();
    
    static Ingredient()
    {
        prefixDictionary.Add((int)PrepMethod.Slice, "슬라이스된 ");
        prefixDictionary.Add((int)PrepMethod.Chop, "채썰기한 ");
        
    }

    public static string GetPrefix(int method)
    {
        return prefixDictionary[method];
    }
    /*
        TODO

        나중에는 이 녀석에 대한 애니메이션도 추가할 것.

        객체가 IInteractable 로 저장되어 있는 곳으로 쏘오오옥 들어가서

        자신만의 애니메이션을 작동. 그리고 소멸될거임.

        nextForm 의 음식은 IInteractable 에서 직접 지급한다!
    */
}
