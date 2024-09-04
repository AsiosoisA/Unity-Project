using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigamableRestaurantComponent : RestaurantComponent
{
    public RecipeBook recipeBook;

    public virtual void BeforeMinigame()
    {
        Debug.Log("인풋 시스템 감지 차단됨!");
    }

    public virtual void AfterMinigame()
    {
        Debug.Log("인풋 시스템 감지 작동됨!");
    }

    public List<SO_Recipe> GetRecipeListToInteract(List<Order> recipeList)
    {
        List<SO_Recipe> resultList = new List<SO_Recipe>();

        foreach(Order item in recipeList)
        {
            MakeListWithTree(resultList, item.recipe);
        }

        return resultList;
    }

    private void MakeListWithTree(List<SO_Recipe> recipeTree, SO_Recipe nodeRecipe)
    {
        recipeTree.Add(nodeRecipe);
        Debug.Log("MakeListWithTree : " + nodeRecipe.name + "레시피가 추가됐습니다!");

        foreach(FoodStuff recipeIngredient in nodeRecipe.ingredients)
        {

            Debug.Log("감지된 재료 : " + recipeIngredient.foodStuffName);

            if(recipeBook.IsRecipeExist(recipeIngredient.foodStuffName))
            {
                // 날 것의 재료가 아니다. 즉, 레시피가 엄연히 존재하는 음식이다?
                MakeListWithTree(recipeTree, recipeBook.GetRecipe(recipeIngredient.foodStuffName));
            }
        }
    }
}
