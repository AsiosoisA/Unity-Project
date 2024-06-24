using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllFoodList : MonoBehaviour
{
    [SerializeReference]
    List<Food> allFoods;

    [SerializeReference]
    List<Food> lockedFoods;

    [SerializeReference]
    List<Food> unlockFoods;
}
