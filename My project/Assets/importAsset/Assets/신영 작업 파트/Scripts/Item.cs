using UnityEngine;
using System;
using Bardent.Weapons;

[CreateAssetMenu(fileName = "item", menuName = "Data/item Data/item", order = 0)]
public class Item : ScriptableObject
{
    [field: SerializeField] public string itemName;
    [field: SerializeField] public Sprite icon;
    [field: SerializeField] public ItemType itemType;
    [field: SerializeField] public float weight;
    [field: SerializeField] public int count;
    [field: SerializeField] public WeaponDataSO weaponData;
    public enum ItemType
    {
        Weapon,
        ingredients
    }
}