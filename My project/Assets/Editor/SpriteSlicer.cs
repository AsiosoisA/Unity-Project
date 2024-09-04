using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.U2D.Sprites;
using UnityEditorInternal;
using UnityEngine.U2D;
using UnityEditor.U2D;

public class SpriteSlicer : MonoBehaviour
{
    private static string[] spriteNames = {
        "walk0" , "walk1" , "walk2" , "walk3" , "walk4" , "walk5" ,              "empty" , "empty2",
        "back_sitDown0", "back_sitDown1", "back_sitDown2" ,         "back_wait"       ,      "back_eat0", "back_eat1" , "back_eat2" , "back_eat3" ,
        "side_sitDown0", "side_sitDown1", "side_sitDown2" ,         "side_wait"       ,      "side_eat0", "side_eat1" , "side_eat2" , "side_eat3" ,
    };

    /*TODO 여기에는 이 툴을 실행했을 때 실제로 슬라이스 작업을 수행할 파일의 경로를 작성합니다!*/
    private static string folderPath = "Assets/Editor/TargetFolder/SpritesToSlice/";

    [MenuItem("Tools/Slice Sprites And Make Atlas")]
    static void SliceSprites()
    {
        string[] files = Directory.GetFiles(folderPath, "*.png", SearchOption.AllDirectories);

        foreach(string file in files)
        {
            #region 복잡한 파일 경로로부터 스프라이트, 스프라이트 설정 관리자 가져오는 코드
            string assetPath = file.Replace(Application.dataPath, "Assets"); // 경로를 Asset Database 형식으로 바꿔야 함. Assets/뭐시깽이 이렇게 되게!
            Debug.Log("Asset Path : " + assetPath);

            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);

            SpriteDataProviderFactories factory = new SpriteDataProviderFactories();
            factory.Init();
            var dataProvider = factory.GetSpriteEditorDataProviderFromObject(texture);
            dataProvider.InitSpriteEditorDataProvider();
            TextureImporter textureImporter = dataProvider.targetObject as TextureImporter;
            
            #endregion

            // 스프라이트 파일과 그 파일의 설정 관리자를 가져왔으니 본격적으로 슬라이스 설정을 해보겠습니다!!!
            
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spriteImportMode = SpriteImportMode.Multiple; // 픽셀 모드 : 멀티플
            textureImporter.spritePixelsPerUnit = 32; // 픽설 퍼 유닛 : 32
            textureImporter.filterMode = FilterMode.Point; // 필터 모드 : 포인트
            textureImporter.textureCompression = TextureImporterCompression.Uncompressed; // Compression : None
            textureImporter.SaveAndReimport();

            ITextureDataProvider textureProvider = dataProvider.GetDataProvider<ITextureDataProvider>();
            if(textureProvider != null)
            {
                int width = 0, height = 0;
                textureProvider.GetTextureActualWidthAndHeight(out width, out height);
                Rect[] rects = InternalSpriteUtility.GenerateGridSpriteRectangles(texture, Vector2.zero, new Vector2(50, 58), Vector2.zero, true);

                List<SpriteRect> rectangles = new List<SpriteRect>();
                for(int i = 0; i < rects.Length; i++)
                {
                    SpriteRect rectangle = new SpriteRect();

                    rectangle.rect = rects[i];

                    rectangle.alignment = SpriteAlignment.BottomCenter;

                    rectangle.name = spriteNames[i];
                    rectangle.pivot = new Vector2(0f, 0.5f);
                    rectangle.spriteID = GUID.Generate();
                    rectangles.Add(rectangle);
                }
                dataProvider.SetSpriteRects(rectangles.ToArray());
                dataProvider.Apply();
            }
            textureImporter.SaveAndReimport();

            //CreateSpriteAtlas(assetPath);
        }
    }

    static void CreateSpriteAtlas(string assetPath)
    {
        string spriteSheetName = Path.GetFileName(assetPath);
        string atlasPath = folderPath + spriteSheetName + ".spriteatlas";

        SpriteAtlas atlas = new SpriteAtlas();

        //아틀라스 설정
        SpriteAtlasTextureSettings textureSettings = new SpriteAtlasTextureSettings()
        {
            filterMode = FilterMode.Point
        };
        atlas.SetTextureSettings(textureSettings);

        TextureImporterPlatformSettings platformSettings = new TextureImporterPlatformSettings
        {
            textureCompression = TextureImporterCompression.Uncompressed
        };
        atlas.SetPlatformSettings(platformSettings);  

        AssetDatabase.CreateAsset(atlas, atlasPath);

        Object[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath);
        atlas.Add(sprites);        

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
