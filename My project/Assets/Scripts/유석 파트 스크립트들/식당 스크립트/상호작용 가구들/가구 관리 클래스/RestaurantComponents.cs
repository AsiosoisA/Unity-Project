using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RestaurantComponents : MonoBehaviour
{
    #region 식당에 얹어질 가구들
    public List<Table> level_1_tables;
    public List<Table> level_2_added_tables;
    public List<Table> level_3_added_tables;
    #endregion

    void Awake(){
        InitComponents();
    }

    private void InitComponents(){
        List<RestaurantComponent> comps = new List<RestaurantComponent>();

        comps.AddRange(level_1_tables);
        comps.AddRange(level_2_added_tables);
        comps.AddRange(level_3_added_tables);

        foreach(RestaurantComponent comp in comps){
            comp.SetContainer(this);
        }
    }
}
