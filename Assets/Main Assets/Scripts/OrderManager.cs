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
        [SerializeField] private GameObject orderPrefab;
        [SerializeField] private Transform orderSpawnPos;
        [SerializeField] private bool allowRepeatedIngredients;
        
        private System.Random _rnd;
        private Dictionary<Ingredient, int> _order;
        private List<Ingredient> _availableIngredients;
        
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
            foreach (Ingredient ingredient in pizzaDelivered.AttachedIngredients)
            {
                if(!pizzaIngredients.TryAdd(ingredient, 1)) pizzaIngredients[ingredient]++;
            }

            foreach (Ingredient ingredient in pizzaIngredients.Keys)
            {
                if(!order.TryGetValue(ingredient, out int amount)) _errorsMade++;
                if(amount != pizzaIngredients[ingredient]) _errorsMade+= math.abs(amount - pizzaIngredients[ingredient]);
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
        
        public void GenerateOrder(int orderNumber)
        {
            _availableIngredients = new List<Ingredient>(validIngredients);
            _order = new Dictionary<Ingredient, int>();
            int ingredientsNumber = _rnd.Next(1, maxIngredientNumber);

            for (int i = 0; i < ingredientsNumber; i++)
            {
                Ingredient newIngredient = _availableIngredients[_rnd.Next(0, _availableIngredients.Count)];
                int amount = newIngredient.SnapToPizza ? 1 : _rnd.Next(1, maxPerIngredientAmount);

                if(!_order.ContainsKey(newIngredient))
                    _order.Add(newIngredient, amount);
                
                else
                {
                    _order[newIngredient] += amount;
                }
                if(newIngredient.SnapToPizza || !allowRepeatedIngredients) _availableIngredients.Remove(newIngredient);
            }
            
            var newOrder = Instantiate(orderPrefab, orderSpawnPos.position, Quaternion.identity);
            newOrder.GetComponent<Order>().SetOrder(_order, orderNumber);
        }
    }
}