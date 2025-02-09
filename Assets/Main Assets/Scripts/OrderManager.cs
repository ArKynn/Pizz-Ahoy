using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Main_Assets.Scripts
{
    public class OrderManager : MonoBehaviour
    {
        [SerializeField] private Ingredient[] validIngredients;
        [SerializeField] private int maxIngredientNumber;
        [SerializeField] private int maxPerIngredientAmount;
        
        private System.Random _rnd;
        private Dictionary<Ingredient, int> _order;
        
        public Pizza pizzaDelivered { get; private set; }
        public Dictionary<Ingredient, int> order { get; private set; }
        
        private int _errorsMade;
        
        private void Start()
        {
            _rnd = new System.Random();
        }

        public int DeliverPizza(Pizza pizza, Dictionary<Ingredient, int> deliveredOrder)
        {
            pizzaDelivered = pizza;
            order = deliveredOrder;

            return CheckOrderCorrect();
        }

        private int CheckOrderCorrect()
        {
            Dictionary<Ingredient, int> pizzaIngredients = new Dictionary<Ingredient, int>();
            foreach (Ingredient ingredient in pizzaDelivered.GetIngredients())
            {
                if(!pizzaIngredients.TryAdd(ingredient, 1)) pizzaIngredients[ingredient]++;
            }

            foreach (Ingredient ingredient in pizzaIngredients.Keys)
            {
                if(!order.TryGetValue(ingredient, out int amount)) _errorsMade++;
                if(amount != pizzaIngredients[ingredient]) _errorsMade+= math.abs(amount - order[ingredient] - 1);
            }
            
            if(pizzaDelivered.CookState == Pizza.State.Cooked)
            {
                foreach (Ingredient ingredient in pizzaIngredients.Keys)
                {
                    if(ingredient.CookState != Ingredient.State.Cooked) _errorsMade++;
                }
            }
            else
            {
                _errorsMade++;
            }
            
            return _errorsMade;
        }
        
        public Dictionary<Ingredient, int> GenerateOrder()
        {
            _order = new Dictionary<Ingredient, int>();

            for (int i = 0; i < _rnd.Next(1, maxIngredientNumber); i++)
            {
                _order.Add(validIngredients[_rnd.Next(0, validIngredients.Length)], _rnd.Next(1, maxPerIngredientAmount));
            }
            
            return _order;
        }
    }
}