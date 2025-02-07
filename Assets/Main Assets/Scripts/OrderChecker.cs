using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Main_Assets.Scripts
{
    public class OrderChecker : MonoBehaviour
    {
        public Pizza PizzaDelivered { get; private set; }
        public Dictionary<Ingredient, int> Order { get; private set; }
        
        private int errorsMade;

        public int DeliverPizza(Pizza pizza, Dictionary<Ingredient, int> order)
        {
            PizzaDelivered = pizza;
            Order = order;

            return CheckOrderCorrect();
        }

        private int CheckOrderCorrect()
        {
            Dictionary<Ingredient, int> pizzaIngredients = new Dictionary<Ingredient, int>();
            foreach (Ingredient ingredient in PizzaDelivered.GetIngredients())
            {
                if(!pizzaIngredients.TryAdd(ingredient, 1)) pizzaIngredients[ingredient]++;
            }

            foreach (Ingredient ingredient in pizzaIngredients.Keys)
            {
                if(!Order.TryGetValue(ingredient, out int amount)) errorsMade++;
                if(amount != pizzaIngredients[ingredient]) errorsMade+= math.abs(amount - Order[ingredient] - 1);
            }
            
            if(PizzaDelivered.CookState == Pizza.State.Cooked)
            {
                foreach (Ingredient ingredient in pizzaIngredients.Keys)
                {
                    if(ingredient.CookState != Ingredient.State.Cooked) errorsMade++;
                }
            }
            else
            {
                errorsMade++;
            }
            
            return errorsMade;
        }
    }
}