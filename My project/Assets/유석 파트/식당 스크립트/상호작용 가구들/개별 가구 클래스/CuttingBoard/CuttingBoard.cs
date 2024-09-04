using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CuttingBoard : CookableRestaurantComponent
{
    public enum HowToCook
    {
        Slice,
        Chop,
        MakeSushi
    }

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer ingredientSpriteRenderer;

    public GameObject ingredientPrepped;

    //public SO_SequencialKeyMinigame sliceMinigameData;
    //[SerializeField] private List<Sprite> sliceFrames = new List<Sprite>();
    public SO_MinigameAndAnimationData sliceMinigame_Level1;
    public SO_MinigameAndAnimationData sliceMinigame_Level2;

    //public SO_SequencialKeyMinigame chopMinigameData;
    public SO_MinigameAndAnimationData makeSushiMinigame_Level1;


    public SO_MinigameAndAnimationData dataForMinigame {get; private set;}


    private SO_MinigameData minigameToReturn;
    private List<Sprite> frameToAnimate;
    private List<Sprite> frameToAnimate_ingredient;
    private Sprite originalSprite;
    private Sprite ingredientOriginalSprite;
    private int frameIndex;

    protected override void Awake()
    {
        base.Awake();

        spriteRenderer = GetComponent<SpriteRenderer>();

        ingredientSpriteRenderer = ingredientPrepped.GetComponent<SpriteRenderer>();

        Debug.Log("ingredientSpriteRenderer 의 게임오브젝트 이름 : " + ingredientSpriteRenderer.gameObject.name);

        structureName = "cuttingBoard";

        originalSprite = spriteRenderer.sprite;

        ingredientOriginalSprite = ingredientSpriteRenderer.sprite;
    }

    protected override void ShowInitialSprite()
    {
        base.ShowInitialSprite();

        spriteRenderer.sprite = frameToAnimate[0];
        ingredientSpriteRenderer.sprite = frameToAnimate_ingredient[0];
    }

    public override SO_MinigameData GetMinigameAndSetSprites(int howToCook)
    {
        frameIndex = 0;

        if(howToCook == (int)HowToCook.Slice)
        {
            if(restaurant.player.debug_playerSliceLevel == 1) dataForMinigame = sliceMinigame_Level1;
            else if(restaurant.player.debug_playerSliceLevel >= 2) dataForMinigame = sliceMinigame_Level2;
        } 
        else if(howToCook == (int)HowToCook.Chop)
        {

        }
        else if(howToCook == (int)HowToCook.MakeSushi)
        {
            dataForMinigame = makeSushiMinigame_Level1;
        }
        else
        {
            Debug.LogError("올바르지 않은 접근입니다!");
            return null;
        }

        frameToAnimate = dataForMinigame.spriteFrames;
        minigameToReturn = dataForMinigame.minigameData;
        
        frameToAnimate_ingredient = MakePartialIngredientSpriteList();

        ShowInitialSprite();

        return minigameToReturn;
    }

    private List<Sprite> MakePartialIngredientSpriteList()
    {
        List<Sprite> result = new List<Sprite>();
        int curDataIndex = 0;

        for(int i = 0 ; i < recipeToCheckInteractable[currentItemIndex].makingSprites.Count; i++)
        {
            if(dataForMinigame.ingredientIndexes[curDataIndex] == i)
            {
                result.Add(recipeToCheckInteractable[currentItemIndex].makingSprites[i]);
                curDataIndex++;
            }
        }

        return result;
    }

    public override void OnKeyInputSuccessed()
    {
        base.OnKeyInputSuccessed();

        Animate();
    }

    public override void OnEntireMinigameFinished()
    {
        spriteRenderer.sprite = originalSprite;
        ingredientSpriteRenderer.sprite = ingredientOriginalSprite;

        base.OnEntireMinigameFinished();
    }  

    private void Animate()
    {
        if(frameIndex == frameToAnimate.Count) frameIndex = 0;

        spriteRenderer.sprite = frameToAnimate[frameIndex];
        ingredientSpriteRenderer.sprite = frameToAnimate_ingredient[frameIndex];
        
        frameIndex++;
    }
}
