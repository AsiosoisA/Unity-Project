using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Animator Controller Item", menuName = "Scriptable Objects/Animator Controller Item")]
public class SO_ACItem : ScriptableObject
{
    [SerializeField] public AnimatorOverrideController AC;
    [SerializeField] public List<string> features; 
}
