using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class RestaurantComponents : MonoBehaviour
{
    #region 식당에 얹어질 가구들
    public List<Table> level_1_tables {get; private set;}
    public List<Table> level_2_added_tables { get; private set;}
    public List<Table> level_3_added_tables {get; private set;}
    [SerializeField] private Transform level_1_tablesContainer;
    [SerializeField] private Transform level_2_tablesContainer;
    [SerializeField] private Transform level_3_tablesContainer;

    private int currentTableId;

    public List<RestaurantComponent> defaultComponentList = new List<RestaurantComponent>();
    #endregion

    void Awake(){
        InitComponents();
    }

    private void InitComponents(){
        List<RestaurantComponent> comps = new List<RestaurantComponent>();
        
        currentTableId = 0;

        level_1_tables = new List<Table>();
        level_2_added_tables = new List<Table>();
        level_3_added_tables = new List<Table>();

        AddTablesInList(level_1_tables , level_1_tablesContainer);
        AddTablesInList(level_2_added_tables, level_2_tablesContainer);
        AddTablesInList(level_3_added_tables, level_3_tablesContainer);

        comps.AddRange(level_1_tables);
        comps.AddRange(level_2_added_tables);
        comps.AddRange(level_3_added_tables);

        foreach(RestaurantComponent comp in comps){
            comp.SetContainer(this);
        }
    }

    private void AddTablesInList(List<Table> list, Transform containerTransform)
    {
        //Debug.Log("트랜스폼 탐색 중 ... 이름 : " + containerTransform.name);
        foreach(Transform transform in containerTransform)
        {
            //Debug.Log("트랜스폼 탐색 중........ 이름 : " + transform.name);
            foreach(Transform childTransform in transform)
            {
                //Debug.Log("트랜스폼 탐색 중 ............... 이름 : " + childTransform.name);

                Table newTable = childTransform.gameObject.GetComponent<Table>();
                newTable.tableId = currentTableId++;
                list.Add(newTable);
                //Debug.Log("테이블이 리스트에 추가됐습니다. : " + newTable.tableId);
            }
        }
    }
}
