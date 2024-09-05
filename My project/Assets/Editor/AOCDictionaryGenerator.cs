using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Animations;
using UnityEditor.Animations;
using UnityEngine.U2D;
using System.IO;

public class AOCDictionaryGenerator : MonoBehaviour
{
    [MenuItem("Tools/AOC Dictionary Generate")]
    public static void Generate()
    {





        
        /*

            이곳에 Sprite 폴더 구조와 완벽히 동일한 구조의 ACItems 폴더를 넣어주시면 됩니다!

        */
        string rootDirectoryToAddDictionary = "Assets/Resources/유석 파트/CustomerResources/DefaultCustomerResources";









        string resultPath = "Assets/Editor/TargetFolder/AOC_Dictionary_Result/SO_ACItemDictionary.asset";

        if (string.IsNullOrEmpty(rootDirectoryToAddDictionary) || !Directory.Exists(rootDirectoryToAddDictionary))
        {
            Debug.LogError("AOC Dictionary Generator Error: 유효하지 않은 경로입니다. rootDirectoryToAddDictionary 경로를 확인하세요.");
            return;
        }




        SO_ACItemDictionary dictionary = ScriptableObject.CreateInstance<SO_ACItemDictionary>();
        Dictionary<string, List<SO_ACItem>> acItemDictionary = new Dictionary<string, List<SO_ACItem>>();

        string[] files = Directory.GetFiles(rootDirectoryToAddDictionary, "*.asset", SearchOption.AllDirectories);


        Debug.Log("딕셔너리 생성기 작동 시작!");

        foreach(var file in files)
        {
            SO_ACItem acItem = AssetDatabase.LoadAssetAtPath<SO_ACItem>(file);

            if(acItem != null)
            {
                string key = Path.GetDirectoryName(file.Substring(rootDirectoryToAddDictionary.Length + 1));

                if(!acItemDictionary.ContainsKey(key))
                {
                    acItemDictionary.Add(key, new List<SO_ACItem>());
                }
                //Debug.Log("딕셔너리에 추가된 아이템 : " + key +":"+ acItem.name);
                acItemDictionary[key].Add(acItem);
            }
        }

        ICollection<string> keys = acItemDictionary.Keys;
        foreach(string key in keys)
        {
            dictionary.dictionaryKeys.Add(key);


            // 아이템 추가할 차례. 추가할 아이템은 List<SO_ACItem> . 근데 List<List<SO_ACItem>> 은 시리얼라이즈가 안 돼서 커스텀 클래스로 만들음.
            // 이 때 SO_ACItems . items == List<SO_ACItem> 이라고 보면 됨. 
            SO_ACItems listContainer = new SO_ACItems();
            listContainer.items = acItemDictionary[key]; // 포장하고
            dictionary.dictionaryItems.Add(listContainer); // 그 포장한 객체를 Items에 추가한다!
        }


        AssetDatabase.CreateAsset(dictionary, resultPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("성공적으로 딕셔너리를 생성했습니다! 노력의 집약체 등장... ㄷㄷ");
        Debug.Log("딕셔너리에 추가된 키들 : ");

        foreach(string key in keys)
        {
            Debug.Log(key);
        }

    }
}
