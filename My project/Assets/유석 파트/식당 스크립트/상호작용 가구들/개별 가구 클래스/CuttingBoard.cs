using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public SO_SequencialKeyMinigame sliceMinigameData;
    public SO_SequencialKeyMinigame chopMinigameData;
    public SO_SequencialKeyMinigame makeSushiMinigameData;

    [SerializeField] private List<Sprite> sliceFrames = new List<Sprite>();
    [SerializeField] private List<Sprite> chopFrames = new List<Sprite>();
    [SerializeField] private List<Sprite> makeSushiFrames = new List<Sprite>();

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
            frameToAnimate = sliceFrames;
            minigameToReturn = sliceMinigameData;
        } 
        else if(howToCook == (int)HowToCook.Chop)
        {
            frameToAnimate = chopFrames;
            minigameToReturn = chopMinigameData;
        }
        else if(howToCook == (int)HowToCook.MakeSushi)
        {
            frameToAnimate = makeSushiFrames;
            minigameToReturn = makeSushiMinigameData;
        }
        else
        {
            Debug.LogError("올바르지 않은 접근입니다!");
            return null;
        }
        
        frameToAnimate_ingredient = recipeToCheckInteractable[currentItemIndex].makingSprites;

        if(frameToAnimate.Count != frameToAnimate_ingredient.Count)
        {
            Debug.LogError("조리 스프라이트와 재료 스프라이트간 개수가 안 맞습니다! " + frameToAnimate.Count + " vs " + frameToAnimate_ingredient);
        }

        ShowInitialSprite();

        return minigameToReturn;
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
