using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeBook : MonoBehaviour
{
    /*
        레시피 북은 만들 수 있는 모든 레시피를 가지고 있다.



    */
    public List<SO_Recipe> recipeList = new List<SO_Recipe>();

    public Dictionary<string, SO_Recipe> book = new Dictionary<string, SO_Recipe>();

    void Awake()
    {
        foreach(SO_Recipe recipe in recipeList)
        {
            Debug.Log("레시피북에 추가할 것 : " + recipe.result.foodStuffName + ", 그에 대한 레시피");
            book.Add(recipe.result.foodStuffName, recipe);
        }
    }
    public bool IsPrimitive(string foodName) => !IsRecipeExist(foodName);
    public bool IsRecipeExist(string foodName)
    {
        return book.ContainsKey(foodName);
    }
    public SO_Recipe GetRecipe(string foodName)
    {
        if(!IsRecipeExist(foodName))
        {
            Debug.LogError(foodName + "이라는 이름의 음식을 만드는 레시피는 존재하지 않습니다!");
            return null;
        } 
        else return book[foodName];
    }

    public List<FoodStuff> GetPrimitiveIngredients(SO_Recipe recipe)
    {
        List<FoodStuff> result = new List<FoodStuff>();

        foreach(FoodStuff food in recipe.ingredients)
        {
            if(IsPrimitive(food.foodStuffName)) result.Add(food);
            else
            {
                SO_Recipe foodsRecipe = GetRecipe(food.foodStuffName);
                List<FoodStuff> middleStepList = GetPrimitiveIngredients(foodsRecipe);
                result.AddRange(middleStepList);
            } 
        }

        return result;
    }
}
