using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class CookableRestaurantComponent : MinigamableRestaurantComponent, IInteractableStructure, IMinigamable
{
    private GameObject requesterGameObject;
    private RestaurantInventory reqRestInventory;
    private PlayerInteractState requestState;

    public MinigameManager minigameManager;
    private SO_MinigameData minigameToDo;


    public float xOffset;
    public float yOffset;
 

    protected List<SO_Recipe> recipeToCheckInteractable;
    protected int currentItemIndex;

    public void Interact(PlayerInteractState state, Player requester)
    {
        this.reqRestInventory = requester.restaurantInventory;
        this.requesterGameObject = requester.gameObject;
        this.requestState = state;

        if(reqRestInventory.foodsICarrying.Keys.Count == 0)
        {
            state.OnInteractFinished(); // 들고 있는게 아예 없다면 그냥 종료한다.
            return;
        } 

        // 일단 다른 인풋 시스템을 모두 차단해야 함!
        BeforeMinigame();

        
        if(IsShouldHidePlayer())
        {
            requesterGameObject.SetActive(false);
        }
        
        recipeToCheckInteractable = GetRecipeListToInteract(reqRestInventory.holdingOrders);
        Debug.Log("만들어진 레시피 크기 : " + recipeToCheckInteractable.Count);
        foreach(SO_Recipe recipe in recipeToCheckInteractable)
        {
            Debug.Log("만들어진 리스트에 포함된 레시피 : " + recipe.name);
        }

        currentItemIndex = 0;

        // 상호작용 요청을 받은 이 시점에서 첫 요소부터 하나씩 미니게임을 수행, 다음 단계 식재료로 변환할거임.
        CheckCurrentItemShouldInteract();
    }

    public GameObject GetGameObject() => gameObject;

    public Vector3 GetContainerOffset() => new(xOffset, yOffset, 0);

    protected virtual void ShowInitialSprite(){}

    public bool IsShouldHidePlayer()
    {
        return true;
    }

    private void CheckCurrentItemShouldInteract()
    {
        if(currentItemIndex == recipeToCheckInteractable.Count)//모든 아이템이 전부 처리됐다!
        {
            OnEntireMinigameFinished();
        }
        
        else if( ! (recipeToCheckInteractable[currentItemIndex].whereToCook == structureName) || !IsPlayerGetIngredients(reqRestInventory, recipeToCheckInteractable[currentItemIndex]))
        {
            Debug.Log("이 레시피는 그냥 넘겨도 되는 레시피군요. 넘기겠습니다!");
            // 이 경우 걍 넘기면 됨.
            currentItemIndex++;

            CheckCurrentItemShouldInteract();
        }
        else
        {
            // 아이템이 수행해야 하는 미니게임을 선택한다.
            
            // 레시피에 이 식재료는 어떻게 해야 한다는 정보가 있을거임.
            
            int howToCook = recipeToCheckInteractable[currentItemIndex].howToCook;

            minigameToDo = GetMinigameAndSetSprites(howToCook);

            // 음식의 손질 난이도를 확인, 또한 플레이어의 해당 스킬 숙련도를 확인. 종합적으로 게임의 난이도를 결정하여 설정한다.
                    // ㄴ 이건 그냥 나중에 하자. TODO

            // 그 게임의 난이도 결정에 끝났다면 드디어

            RequestToStartMinigame();
        }
    }

    public virtual SO_MinigameData GetMinigameAndSetSprites(int howToCook)
    {
        return default;
    }

    public virtual void RequestToStartMinigame() => minigameManager.CreateAndStartMinigame(this, minigameToDo);

    public virtual void OnKeyInputSuccessed(){}

    public virtual void OnMyMinigameFinished(string minigameName)
    {
        // 이 함수는 미니게임이 완료되면 호출될거임.

        // 무튼 이 함수가 호출됐다는건 미니게임을 성공해서 이제 음식을 트랜스폼할 수 있으니까

        foreach(FoodStuff ingredient in recipeToCheckInteractable[currentItemIndex].ingredients)
        {
            string ingredientName = ingredient.foodStuffName;
            reqRestInventory.SubFoodFromPlayer(ingredientName, 1);
        }

        FoodStuff nextLevelFood = recipeToCheckInteractable[currentItemIndex].result;
        int nextLevelFoodCount = recipeToCheckInteractable[currentItemIndex].resultCount;

        reqRestInventory.AddFoodToPlayer(nextLevelFood, nextLevelFoodCount);
    
        currentItemIndex++;
        CheckCurrentItemShouldInteract(); 
    }

    public virtual void OnMyMinigameFailed(string minigameName)
    {
        // 어떤 이유든 실패했다? 그럼 절대 보상을 제공해서는 안 된다!
        currentItemIndex++;
        CheckCurrentItemShouldInteract();
    }

    public virtual void OnEntireMinigameFinished()
    {
        requesterGameObject.SetActive(true);
        
        OnInteractFinished();
    }

    public bool IsPlayerGetIngredients(RestaurantInventory inventory, SO_Recipe recipe)
    {
        Debug.Log("감지된 레시피 : " + recipe.name);

        foreach(FoodStuff ingredientName in recipe.ingredients)
        {
            if(!inventory.foodsICarrying.ContainsKey(ingredientName.foodStuffName))
            {
                Debug.Log(ingredientName + "이 없어 요리하지 못 합니다!");
                return false;
            } 
        }
        Debug.Log("식재료가 충분합니다. 요리할 수 있습니다!");
        return true;
    }

    public void OnInteractFinished()
    {
        requestState.OnInteractFinished();
    }
}
