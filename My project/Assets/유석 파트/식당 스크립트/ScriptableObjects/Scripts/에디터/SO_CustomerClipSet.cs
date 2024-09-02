using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new customer clip set", menuName = "Scriptable Objects/Restaurant/CustomerClipSet")]
public class SO_CustomerClipSet : ScriptableObject
{
    [Header("Walk 애니메이션에 쓸 클립")]
    public AnimationClip walkClip;

    [Header("SitDown 애니메이션에 쓸 클립")]
    public AnimationClip sitDownClip;

    [Header("wait 애니메이션에 쓸 클립")]
    public AnimationClip waitClip;

    [Header("eat 애니메이션에 쓸 클립")]
    public AnimationClip eatClip;

    [Header("SitUp 애니메이션에 쓸 클립")]
    public AnimationClip sitUpClip;
}
