using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Animations;
using UnityEditor.Animations;
using UnityEngine.U2D;
using Unity.VisualScripting;

public class AOCGenerator : MonoBehaviour
{
    [MenuItem("Tools/Animation Override Controller Generate")]
    public static void Generate(){

        string originalACPath = "Assets/Editor/TargetFolder/AOC_Generator/OriginalAC/";
        string spriteSheetPath = "Assets/Editor/TargetFolder/AOC_Generator/SpriteSheet/";
        string ResultPath = "Assets/Editor/TargetFolder/AOC_Generator/Result/";

        AnimatorController originalController;

        List<Dictionary<string, Sprite>> spriteSheets = new List<Dictionary<string, Sprite>>();
        List<string> spriteNames = new List<string>();

        Dictionary<string, List<string>> clipDictionary = new Dictionary<string, List<string>>();

        #region 애니메이터 불러오기
        string[] guids = AssetDatabase.FindAssets("t:AnimatorController", new[] {originalACPath});
        
        if(guids.Length != 1)
        {
            Debug.LogError("AOC Generator Error : OriginalAC 파일에 한 개의 원본만 넣어주세요!");
            return;
        }

        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        originalController = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
        #endregion

        #region 스프라이트 시트 불러오기
        guids = AssetDatabase.FindAssets("t:Texture2D", new[] {spriteSheetPath});

        if(guids.Length == 0)
        {
            Debug.LogError("AOC Generator Error : SpriteSheet 파일에 스프라이트를 넣어주세요!");
            return;
        }

        for(int i = 0 ; i < guids.Length; i++)
        {
            path = AssetDatabase.GUIDToAssetPath(guids[i]);

            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

            if(texture != null)
            {
                spriteNames.Add(texture.name);
                //Debug.Log("texture2D를 불러오는 데에 성공! 텍스처 이름 : " + texture.name);

                var assets = AssetDatabase.LoadAllAssetsAtPath(path);

                if(assets != null && assets.Length > 0)
                {
                    List<Sprite> spriteSheet = new List<Sprite>();
                    foreach(var asset in assets)
                    {
                        Sprite newItem = asset as Sprite;
                        if(newItem != null)
                        {
                            spriteSheet.Add(newItem);
                        }
                    }

                    Dictionary<string, Sprite> spriteDictionary = new Dictionary<string, Sprite>();
                    for(int j = 0; j < spriteSheet.Count; j++)
                    {
                        spriteDictionary.Add(spriteSheet[j].name, spriteSheet[j]);
                    }
                    spriteSheets.Add(spriteDictionary);
                }
                else
                {
                    Debug.LogError("AOCGenerator Error : 스프라이트 시트를 불러오는데 실패했습니다... " + path);

                    if(assets == null)
                    {
                        Debug.LogError("에러 코드 : 불러온 스프라이트 시트가 null 입니다.");
                    }
                    else
                    {
                        Debug.LogError("에러 코드 : 스프라이트 시트에 스프라이트가 저장되지 않았습니다.");
                    }

                    return;
                }
            }
        }
        #endregion

        #region 애니메이터 컨트롤러에서 클립에 쓰인 스프라이트 이름 추출하기
        for(int i = 0; i < originalController.animationClips.Length; i++)
        {
            AnimationClip clip = originalController.animationClips[i];
            clipDictionary.Add(clip.name, new List<string>());

            var bindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);

            foreach(var binding in bindings)
            {
                ObjectReferenceKeyframe[] keyframes = AnimationUtility.GetObjectReferenceCurve(clip, binding);

                foreach(var keyframe in keyframes)
                {
                    Sprite sprite = keyframe.value as Sprite;

                    if(sprite != null)
                    {
                        clipDictionary[clip.name].Add(sprite.name);
                    }
                }
            }
        }
        #endregion

        #region 스프라이트 시트 개수만큼 복제된 애니메이터 오버라이드 컨트롤러 만들기
        for(int i = 0; i < spriteSheets.Count; i++)
        {
            //string overrideControllerPath = ResultPath + spriteNames[i] + ".controller";
            string aocItemPath = ResultPath + spriteNames[i] + ".asset";

            AnimatorOverrideController overrideController = new AnimatorOverrideController(originalController);
            overrideController.name = spriteNames[i];

            //AssetDatabase.CreateAsset(overrideController, overrideControllerPath);
            //AssetDatabase.SaveAssets();

            SO_ACItem so_acItem = ScriptableObject.CreateInstance<SO_ACItem>();
            so_acItem.AC = overrideController;
            AssetDatabase.CreateAsset(so_acItem, aocItemPath);
            AssetDatabase.SaveAssets();

        
            foreach(AnimationClip clip in overrideController.animationClips)
            {
                AnimationClip newClip = new AnimationClip();
                EditorUtility.CopySerialized(clip, newClip);

                EditorCurveBinding[] bindings = AnimationUtility.GetObjectReferenceCurveBindings(newClip);
                foreach (var binding in bindings)
                {
                    ObjectReferenceKeyframe[] keyframes = AnimationUtility.GetObjectReferenceCurve(newClip, binding);
                    for (int j = 0; j < keyframes.Length; j++)
                    {
                        string spriteName = clipDictionary[clip.name][j /* % clipDictionary[clip.name].Count */];
                        Sprite newSprite = spriteSheets[i][spriteName];

                        if(newSprite == null)
                        {
                            Debug.LogError("스프라이트를 받아올 수 없었습니다! 받아오려고 시도한 스프라이트 이름 : " + spriteName);
                        }
                        else {
                            keyframes[j].value = newSprite;
                        }
                    }
                    AnimationUtility.SetObjectReferenceCurve(newClip, binding, keyframes);
                }

                overrideController[clip.name] = newClip;
            }

            AssetDatabase.SaveAssets();
        }

        #endregion

        Debug.Log("성공적으로 AOC를 생성했습니다! 아마도.");
    }
}
