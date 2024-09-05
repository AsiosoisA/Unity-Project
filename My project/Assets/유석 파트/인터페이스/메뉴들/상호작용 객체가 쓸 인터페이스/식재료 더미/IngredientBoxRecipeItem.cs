using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class IngredientBoxRecipeItem : MonoBehaviour
{

    public IngredientBoxInterface myInterfaceMenu;

    public Order item;
    public Image resultFoodImage;
    public Text resultFoodText;
    public Button button;


    public RectTransform rectTransform {get; set;}

    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Init(IngredientBoxInterface menu, Order order)
    {
        myInterfaceMenu = menu;
        SetOrder(order);
    }

    public void SetOrder(Order order)
    {
        this.item = order;

        resultFoodImage.sprite = order.recipe.result.foodSprite;
        resultFoodText.text = order.recipe.result.foodStuffName;
    }

    public void OnClick()
    {
        List<FoodStuff> foodsToGive = myInterfaceMenu.box.book.GetPrimitiveIngredients(item.recipe);

        //TODO 이거 만약 재고량이 부족하면 못 하게 막아야 한다!!
        if(!myInterfaceMenu.box.boxInventory.CanISubThis(foodsToGive))
        {
            Debug.Log("재고가 부족합니다!");
            PopupManager.Instance.MakePopup("재고가 부족합니다!");
            return;
        }
        else
        {
            foreach(FoodStuff food in foodsToGive)
            {
                myInterfaceMenu.box.boxInventory.SubFoodFromPlayer(food.foodStuffName, 1);
                myInterfaceMenu.box.playerInventory.AddFoodToPlayer(food, 1);
            }

            item.isPullIngredients = true;

            myInterfaceMenu.Sync();
        }
    }
}
