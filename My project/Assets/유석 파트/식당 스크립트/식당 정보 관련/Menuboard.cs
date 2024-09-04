using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menuboard : MonoBehaviour
{
    [SerializeReference]
    public List<Food> menu;

    public void InsertFood(Food food){
        menu.Add(food);
    }

    public void RemoveFood(string foodName){ // 음식 이름으로 제거
        menu.RemoveAll(food => food.foodStuffName == foodName);
    }

    public Food suggestFood(){
        int foodIdx = Random.Range(0, menu.Count);
        return menu[foodIdx];
    }
}
