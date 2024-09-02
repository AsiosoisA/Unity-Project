using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientBoxPlayerItem : MonoBehaviour
{
    public FoodStuff item;

    public RectTransform rectTransform {get; set;}

    public Image itemImage;
    public Text itemCount;
    public Button button;

    void Awake()
    {
        this.rectTransform = GetComponent<RectTransform>();
    }

    public void InitCell(FoodStuff item, int count)
    {
        this.item = item;

        itemImage.sprite = item.foodSprite;
        itemCount.text = ""+ count;


    }
}
