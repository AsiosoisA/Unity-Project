using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    public Food foodIWant;
    public Customer orderer;
    public SO_Recipe recipe;


    public bool isPullIngredients; 


    public Order(Customer orderer, Food food)
    {
        this.orderer = orderer;
        foodIWant = food;
        isPullIngredients = false;
    }
}
