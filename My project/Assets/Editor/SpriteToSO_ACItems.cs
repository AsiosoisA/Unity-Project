using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Animations;
using UnityEditor.Animations;
using UnityEngine.U2D;
using System.IO;

public class SpriteToSO_ACItems : MonoBehaviour
{
    [MenuItem("Tools/AllInOne/CopySpritesAndPasteToSO_ACItems")]
    public static void CopySpritesAndPasteToSO_ACItems()
    {
        
        //////////////////////////             CONFIG               ///////////////////////////////////
        /*
            여기다 Sprites 폴더 주소를 넣어주세요!
        */
        string spriteDirectoryPath = "Assets/Resources/유석 파트/CustomerResources/Sprites";
        /*
            여기다 ACItems 폴더 주소를 넣어주세요!
        */
        string targetDirectoryPath = "Assets/Resources/유석 파트/CustomerResources/DefaultCustomerResources";
        /*
            여기다 베이스로 쓸 AC 의 주소를 넣어주세요!
        */
        string baseACPath = "Assets/유석 파트/애니메이션/애니메이션 컨트롤러/손님";


        bool isOverrideFeaturedAOC = false; // ACItem 을 생성할 때 Feature 가 지정된 ACItem 까지도 덮어써버릴지 여부.


        ////////////////////////////////////////////////////////////////////////////////////////////////



        string sourceDirectory = Application.dataPath + spriteDirectoryPath.Substring("Assets".Length);
        string TargetDirectory = Application.dataPath + targetDirectoryPath.Substring("Assets".Length);


        #region 입력받은 두 디렉토리의 구조가 완전히 동일한지부터 체크.


        var folders1 = Directory.GetDirectories(sourceDirectory, "*", SearchOption.AllDirectories);
        var folders2 = Directory.GetDirectories(TargetDirectory, "*", SearchOption.AllDirectories);

        var path2RelativePaths = new HashSet<string>();
        foreach(var folder in folders2)
        {
            var relativePath = folder.Substring(TargetDirectory.Length).TrimStart(Path.DirectorySeparatorChar);
            path2RelativePaths.Add(relativePath);
        }

        foreach(var folder in folders1)
        {
            var relativePath = folder.Substring(sourceDirectory.Length).TrimStart(Path.DirectorySeparatorChar);
            if(!path2RelativePaths.Contains(relativePath))
            {
                Debug.LogError("두 파일의 디렉토리 구조가 동일하지 않습니다! 동일하지 않은 경로 : " + relativePath);
                return;
            }
        }
        #endregion

        Debug.Log("두 디렉토리 경로가 일치함을 확인! AOC 데이터 생성을 시작합니다.");


        #region AOC 생성
        AnimatorController originalController;
        List<Dictionary<string, Sprite>> spriteSheets = new List<Dictionary<string, Sprite>>();
        List<string> spriteNames = new List<string>();
        Dictionary<string, List<string>> clipDictionary = new Dictionary<string, List<string>>();
        
        #region 애니메이터 불러오기
        string[] guids = AssetDatabase.FindAssets("t:AnimatorController Base", new[] {baseACPath});

        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        originalController = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
        #endregion


        /*
            이 영역에서 다음 동작들이 이루어짐.

            texture2DFilesPath 에 spriteDirectoryPath 의 하위 경로들, 즉 스프라이트 시트들의 경로들이 저장됨.
            spriteNames 에 해당 스프라이트 시트의 이름이 기록됨.
            spriteSheets 에 해당 스프라이트 시트에 있는 스프라이트들이 모두 저장됨.
        */
        #region 모든 스프라이트 시트 가져오기
        var texture2DFilesAbPath = Directory.GetFiles(sourceDirectory, "*.png", SearchOption.AllDirectories);

        string[] texture2DFilesPath = new string[texture2DFilesAbPath.Length];
        for(int i = 0; i < texture2DFilesAbPath.Length; i++)
        {
            texture2DFilesPath[i] = texture2DFilesAbPath[i].Substring(Application.dataPath.Length - "Assets".Length).TrimStart(Path.DirectorySeparatorChar);
            // 이제 경로가 Assets/Resources/... 이런 식으로 될거임. AssetDatabase 에서 쓸 수 있게 됐다!
        }

        for(int i = 0; i < texture2DFilesPath.Length; i++)
        {
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texture2DFilesPath[i]);
            if(texture == null)
            {
                Debug.LogError("Warning! " + texture2DFilesPath[i] + "에서 texture2D 를 추출할 수 없습니다.");
            }

            spriteNames.Add(texture.name);

            var assets = AssetDatabase.LoadAllAssetsAtPath(texture2DFilesPath[i]);
            if(assets == null || assets.Length <= 0)
            {
                Debug.LogError("Warning! " + texture2DFilesPath[i] + " 파일에서 스프라이트를 추출할 수 없습니다.");
            }
            else if(assets.Length == 1)
            {
                Debug.Log("Warning....??? " + texture2DFilesPath[i] + "에서 겨우 한 개의 스프라이트만 추출됐습니다. 제대로 분할했는지 확인해주십시오. 일단 진행합니다.");
            }

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

        Debug.Log("모든 스프라이트를 불러오는 데에 성공했습니다.");
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


        #region 애니메이터 오버라이드 컨트롤러 생성해서 올바른 경로에 다 갖다 붙여넣기
        for(int i = 0; i < spriteSheets.Count; i++)
        {
            string resultPath = Path.ChangeExtension((targetDirectoryPath + texture2DFilesPath[i].Substring(spriteDirectoryPath.Length)), ".asset");
            //string overrideControllerPath = Path.ChangeExtension((targetDirectoryPath + texture2DFilesPath[i].Substring(spriteDirectoryPath.Length)), ".controller");

            string resourcePath = Path.ChangeExtension(resultPath, "");
            resourcePath = resourcePath.Substring(0, resourcePath.Length - 1);

            string clipsPath = Path.Combine(resourcePath, "clips");
            if(!AssetDatabase.IsValidFolder(resourcePath))
            {
                AssetDatabase.CreateFolder(Path.GetDirectoryName(resourcePath) , Path.GetFileName(resourcePath));
            }
            if(!AssetDatabase.IsValidFolder(clipsPath))
            {
                AssetDatabase.CreateFolder(resourcePath , "clips");
            }            

            // 중복 체크!! 만약 이미 같은 이름의 SO_ACItem 이 있다면 그냥 건너뛴다.
            #region 중복체크
            SO_ACItem asset = AssetDatabase.LoadAssetAtPath<SO_ACItem>(resultPath);
            if(asset != null)
            {
                if(asset.features == null || asset.features.Count != 0)
                {
                    if(isOverrideFeaturedAOC == false)
                    {
                        Debug.Log("Sprite To SO_ACItems : " + resultPath + "경로에 이미 파일이 존재하므로 " + spriteNames[i] + "스프라이트에 대한 작업은 건너뜁니다.");
                        continue;
                    }
                    else
                    {
                        AssetDatabase.DeleteAsset(resultPath);
                    }
                }
                else
                {
                    Debug.Log("이미 "+ spriteNames[i] +"파일이 존재하지만, Feature 가 없으므로 그냥 덮어써버리겠습니다.");
                    AssetDatabase.DeleteAsset(resultPath);
                }
            }
            #endregion
            




            // 여기서 쓸 수 있는 자원 : 
            // spriteNames
            // spriteSheets
            // texture2DFilesPath
            string acTypeToOverride = texture2DFilesPath[i].Substring(spriteDirectoryPath.Length + 1).Split('\\')[0];
            
            #region 애니메이터 불러오기
            guids = AssetDatabase.FindAssets("t:AnimatorController " + char.ToUpper(acTypeToOverride[0]) + acTypeToOverride.Substring(1), new[] {baseACPath});

            string originalAcPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            originalController = AssetDatabase.LoadAssetAtPath<AnimatorController>(originalAcPath);
            
            #endregion


            AnimatorOverrideController overrideController = new AnimatorOverrideController(originalController);




            overrideController.name = spriteNames[i];

            SO_ACItem so_acItem = ScriptableObject.CreateInstance<SO_ACItem>();
            //so_acItem.AC = overrideController;

            // so_acItem.AC = overrideController;
            // AssetDatabase.CreateAsset(so_acItem, resultPath);
            // AssetDatabase.SaveAssets();

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
                AssetDatabase.CreateAsset(newClip, Path.Combine(clipsPath, newClip.name + ".anim"));
                AssetDatabase.SaveAssets();
            }

            Debug.Log("성공적으로 AOC를 생성했습니다! 아마도.");

            AssetDatabase.CreateAsset(overrideController, Path.Combine(resourcePath, overrideController.name + ".controller"));

            AssetDatabase.SaveAssets();
            
            so_acItem.AC = AssetDatabase.LoadAssetAtPath<AnimatorOverrideController>(Path.Combine(resourcePath, overrideController.name + ".controller"));
            AssetDatabase.CreateAsset(so_acItem, resultPath);

            AssetDatabase.SaveAssets();
        }
        
        Debug.Log("모든 작업이 완료됐습니다!");
        
        #endregion

        #endregion
    }
}
