using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search.Providers;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "new recipe data", menuName = "Scriptable Objects/Recipe")]
public class SO_Recipe : ScriptableObject
{
    public FoodStuff result; // 프리팹 넣을 것.
    public int resultCount = 1; // 요리 완료했을 때 제공될 결과물 개수.
    public List<FoodStuff> ingredients;
    public string whereToCook; // 손질할 때 상호작용할 가구의 이름
    public int howToCook;

    public List<Sprite> makingSprites = new List<Sprite>();

    public void AddIngredient(FoodStuff ingredient)
    {
        ingredients.Add(ingredient);
    }

}

[CustomEditor(typeof(SO_Recipe))]
public class SO_RecipeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if(serializedObject == null || serializedObject.targetObject == null) return;

        DrawDefaultInspector();

        SO_Recipe recipe = (SO_Recipe)target;

        GUILayout.Space(10);
        GUILayout.Label("요리 방법 선택");

        EditorGUILayout.BeginVertical("box");

        if(GUILayout.Button("도마 - 슬라이스"))
        {
            recipe.whereToCook = "cuttingBoard";
            recipe.howToCook = (int) CuttingBoard.HowToCook.Slice;
        }
        if(GUILayout.Button("도마 - 채 썰기"))
        {
            recipe.whereToCook = "cuttingBoard";
            recipe.howToCook = (int) CuttingBoard.HowToCook.Chop;
        }
        if(GUILayout.Button("도마 - 초밥 빚기"))
        {
            recipe.whereToCook = "cuttingBoard";
            recipe.howToCook = (int) CuttingBoard.HowToCook.MakeSushi;
        }
        if(GUILayout.Button("화덕 - 고기 굽기"))
        {
            recipe.whereToCook = "firepit";
            recipe.howToCook = (int) Firepit.HowToCook.Roast;
        }

        EditorGUILayout.EndVertical();
    }
}



/*
    레시피를 만든다.

    최종적으로 만들어져야 하는 음식 프리팹 : 프리팹 할당

    "피자" 는 "퍼펙트 피자도우" 가 필요해.
    ingredient 가 "퍼펙트 피자도우" 라고.
    "퍼펙트 피자도우" 는 빵, 치즈, 슬라이스된 고기가 필요해.
        빵은 밀가루, 우유, 달걀이 필요해.
            밀가루는 기본 재료야.
            우유는 기본 재료야.
            달걀은 기본 재료야.
        치즈는 기본 재료야.
        슬라이스된 고기는 고기가 필요해.
            고기는 기본 재료야.

    즉 raw form 을 얻을 때까지 계속 재귀적으로 탐색하면서 플레이어의 인벤토리에 식재료들을 추가해주어야 하는게 식재료 더미!

    그럼 또 이걸 도마가 받았다고 치자.
    도마가 할 일이 있는가?

    "피자"를 만들 수 있는가? X
    "퍼팩트 피자도우" 를 만들 수 있는가? X
        "빵" 을 만들 수 있는가? X
            밀가루 (만들게 없음. 스킵)
            우유 (만들게 없음. 스킵)
            달걀 (만들게 없음. 스킵)
        치즈 (만들게 없음. 스킵)
        슬라이스된 고기를 만들 수 있는가? O
            !!!!!
            재료가 있는가? O
            !!!!!!!!!!!!!!!!!!!!
            즉시 만들어버림
    
    베이커리가 할 일이 있는가?
    "피자"를 만들 수 있는가? O
    !!!!!
    재료가 있는가? X
    "퍼팩트 피자도우" 를 만들 수 있는가? O
    !!!!!
    재료가 있는가? X
        "빵" 을 만들 수 있는가? O
        !!!!!
        재료가 있는가? O
        !!!!!!!!!!!!!!!!!!!
        즉시 만들어버림
        ...
        ...

        ...
    끝

    다시 상호작용을 하면?
    "피자"를 만들 수 있는가? O
    !!!!!
    재료가 있는가? X
    "퍼팩트 피자도우" 를 만들 수 있는가? O
    !!!!!
    재료가 있는가? O
    !!!!!!!!!!!!!!!!!!!
    즉시 만들어버림

    이제 오븐에 가자.
    "피자를 만들 수 있는가?" O
    !!!!!!!
    재료가 있는가? O
    !!!!!!!!!!!!!!!!!
    즉시 만들어버림.

    그럼 딕셔너리고 뭐고 리스트를 이용한 트리 구조로 가야겠구만!      
*/



/*
    예시
    Recipe Pizza
    ㄴ  result = Food Pizza
        ingredients = { bread , cheeze , slicedMeat }
        how to cook = BakeMinigame
        where to cook = oven

    
    이 상태에서 상호작용 가능한 객체들은 자기가 할 일이 있나 재료별로 다 살펴봄.

    result 를 만들 수 있는가? > 아니오
        ingredients 를 만들 수 있는가? > ingredients 의 레시피를 확인하여 체크.
*/