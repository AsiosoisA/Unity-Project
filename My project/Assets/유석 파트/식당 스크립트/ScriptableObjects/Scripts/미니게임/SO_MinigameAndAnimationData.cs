using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum MinigameType
{
    OneByOneSprite,
    OneByOneAnimate,
    JustAnimate
}


[CreateAssetMenu(fileName = "new minigame and animation data" , menuName = "Scriptable Objects/Minigame and Sprites")]
public class SO_MinigameAndAnimationData : ScriptableObject
{
    public SO_MinigameData minigameData;

    public MinigameType minigameType;



    #region 1:1sprite 매칭일 때 나타날 설정값들
    public List<Sprite> spriteFrames;
    public bool isNotUseEverySprites;
    public List<int> ingredientIndexes;
    #endregion
}

[CustomEditor(typeof(SO_MinigameAndAnimationData))]
public class SO_MinigameAndAnimationDataEditor : Editor
{
    SerializedProperty dataProperty;
    SerializedProperty minigameTypeProperty;



    SerializedProperty spriteFrames;
    SerializedProperty isNotUseEverySprites;
    SerializedProperty ingredientIndexes;

    private void OnEnable()
    {
        dataProperty = serializedObject.FindProperty("minigameData");
        minigameTypeProperty = serializedObject.FindProperty("minigameType");



        spriteFrames = serializedObject.FindProperty("spriteFrames");
        isNotUseEverySprites = serializedObject.FindProperty("isNotUseEverySprites");
        ingredientIndexes = serializedObject.FindProperty("ingredientIndexes");
    }

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();
        serializedObject.Update();
        EditorGUILayout.PropertyField(dataProperty);
        EditorGUILayout.PropertyField(minigameTypeProperty);

        SO_MinigameAndAnimationData data = (SO_MinigameAndAnimationData)target;

        // MinigameType이 OneByOneSprite일 때만 spriteFrames 필드를 그립니다.
        if (data.minigameType == MinigameType.OneByOneSprite)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("1:1 스프라이트 매칭 미니게임 설정", EditorStyles.boldLabel);

            if(data.minigameData != null)
            {
                if(data.minigameData is SO_SequencialKeyMinigame)
                {
                    SO_SequencialKeyMinigame sequencialMinigameData = data.minigameData as SO_SequencialKeyMinigame;

                    int keyCount = sequencialMinigameData.keyCount;

                    while(data.spriteFrames.Count < keyCount)
                    {
                        data.spriteFrames.Add(null);
                    }
                    while(data.spriteFrames.Count > keyCount)
                    {
                        data.spriteFrames.RemoveAt(data.spriteFrames.Count - 1);
                    }

                    EditorGUILayout.PropertyField(spriteFrames, new GUIContent("스프라이트 프레임들"), true);
                
                
                
                    // 이제 Making Sprite 에서 특정 부분만 쓴다고 확인됐다면 어디를 쓸건지도 알아내야 함.
                    EditorGUILayout.PropertyField(isNotUseEverySprites, new GUIContent("재료 sp 일부분만 씀?"), true);
                
                    if(data.isNotUseEverySprites)
                    {
                        while(data.ingredientIndexes.Count < keyCount)
                        {
                            data.ingredientIndexes.Add(default);
                        }
                        while(data.ingredientIndexes.Count > keyCount)
                        {
                            data.ingredientIndexes.RemoveAt(data.ingredientIndexes.Count - 1);
                        }

                        EditorGUILayout.PropertyField(ingredientIndexes, new GUIContent("사용할 Making Sprite 인덱스"), true);
                    }
                    else
                    {
                        data.ingredientIndexes.Clear();
                        for(int i = 0; i < keyCount; i++)
                        {
                            data.ingredientIndexes.Add(i);
                        }
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("1:1 스프라이트 대응인데도 키 순서 누르기 미니게임을 할당하지 않았어요!" , MessageType.Warning);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("SO_MinigameData 를 먼저 할당해주세요!" , MessageType.Warning);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
