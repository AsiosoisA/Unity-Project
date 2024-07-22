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
}
