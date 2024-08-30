using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "new keyCustom data", menuName = "Scriptable Objects/Minigame/KeyCustom")]
public class SO_KeyCustom : ScriptableObject
{
    public float keySize = 1.0f; // 디폴트

    [Header("체크시 키 입력에 성공하면 키가 사라집니다.")]
    public bool isDisappearAfterSuccess = true; // Default : 버튼을 누르면 버튼은 사라짐.

    public List<EffectCustom> startEffects = new List<EffectCustom>();
    public List<EffectCustom> failEffects = new List<EffectCustom>();
    public List<EffectCustom> successEffects = new List<EffectCustom>();
    public List<EffectCustom> endEffects = new List<EffectCustom>();

    public void DoEffects(List<EffectCustom> effectCustoms, EffectCore effectCore)
    {
        foreach(EffectCustom effectCustom in effectCustoms)
        {
            if(effectCustom.effectName == "Magnitude")
            {
                float duration = 1f; // Default

                foreach(PropCustom prop in effectCustom.props)
                {
                    if(prop.name == "duration") duration = prop.value;
                }

                effectCore.Magnitude(duration);
            }

            else if (effectCustom.effectName == "FadeIn")
            {
                float duration = 1f; // Default

                foreach(PropCustom prop in effectCustom.props)
                {
                    if(prop.name == "duration") duration = prop.value;
                }

                effectCore.FadeIn(duration);
            }

            else if (effectCustom.effectName == "FadeOut")
            {
                float duration = 1f;

                foreach(PropCustom prop in effectCustom.props)
                {
                    if(prop.name == "duration") duration = prop.value;
                }

                effectCore.FadeOut(duration);
            }

            else if (effectCustom.effectName == "ChangeScaleConst")
            {
                float scaleValue = 1f;
                float duration = 1f;

                foreach(PropCustom prop in effectCustom.props)
                {
                    if(prop.name == "duration") duration = prop.value;
                    else if(prop.name == "scale") scaleValue = prop.value;
                }   

                effectCore.ChangeScaleConst(scaleValue, duration);
            }
        }
    }
}


[System.Serializable]
public class EffectCustom
{
    public string effectName;
    public List<PropCustom> props;

    public EffectCustom(string name)
    {
        this.effectName = name;
        props = new List<PropCustom>();
    }
}

[System.Serializable]
public struct PropCustom
{
    public string name;
    public float value;

    public PropCustom(string name)
    {
        this.name = name;
        value = default;
    }
}

[CustomEditor(typeof(SO_KeyCustom))]
public class SO_KeyCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SO_KeyCustom custom = (SO_KeyCustom)target;

        GUILayout.Space(10); // 위에 약간 공간 추가
        GUILayout.Label("시작 효과 추가", EditorStyles.boldLabel);

        EditorGUILayout.BeginVertical("box");
        ShowEffectButtons("start", custom);
        EditorGUILayout.EndVertical();

        GUILayout.Space(10); // 위에 약간 공간 추가
        GUILayout.Label("실패시 효과 추가", EditorStyles.boldLabel);

        EditorGUILayout.BeginVertical("box");
        ShowEffectButtons("fail", custom);
        EditorGUILayout.EndVertical();

        GUILayout.Space(10); // 위에 약간 공간 추가
        GUILayout.Label("성공 효과 추가", EditorStyles.boldLabel);

        EditorGUILayout.BeginVertical("box");
        ShowEffectButtons("success", custom);
        EditorGUILayout.EndVertical();

        GUILayout.Space(10); // 위에 약간 공간 추가
        GUILayout.Label("종료 효과 추가", EditorStyles.boldLabel);

        EditorGUILayout.BeginVertical("box");
        ShowEffectButtons("end", custom);
        EditorGUILayout.EndVertical();
    }

    public void ShowEffectButtons(string when, SO_KeyCustom custom)
    {
        if(GUILayout.Button("Add Magnitude Effect"))
        {
            EffectCustom effectCustom = new EffectCustom("Magnitude");

            effectCustom.props.Add(new PropCustom("duration"));

            AddThisInList(effectCustom, when, custom);
        }
        
        if(GUILayout.Button("Add FadeIn Effect"))
        {
            EffectCustom effectCustom = new EffectCustom("FadeIn");

            effectCustom.props.Add(new PropCustom("duration"));

            AddThisInList(effectCustom, when, custom);
        }

        if(GUILayout.Button("Add FadeOut Effect"))
        {
            EffectCustom effectCustom = new EffectCustom("FadeOut");

            effectCustom.props.Add(new PropCustom("duration"));

            AddThisInList(effectCustom, when, custom);
        }

        if(GUILayout.Button("Add ChangeScaleConst Effect"))
        {
            EffectCustom effectCustom = new EffectCustom("ChangeScaleConst");

            effectCustom.props.Add(new PropCustom("scale"));
            effectCustom.props.Add(new PropCustom("duration"));

            AddThisInList(effectCustom, when, custom);
        }
    }

    private void AddThisInList(EffectCustom item, string when, SO_KeyCustom custom)
    {
        List<EffectCustom> targetList;

        if(when == "start") targetList = custom.startEffects;
        else if(when == "fail") targetList = custom.failEffects;
        else if(when == "success") targetList = custom.successEffects;
        else if(when == "end") targetList = custom.endEffects;
        else{
            Debug.LogError("when 을 잘 못 기입했습니다. : " + when);
            return;
        }

        targetList.Add(item);
        EditorUtility.SetDirty(custom);
    }
}