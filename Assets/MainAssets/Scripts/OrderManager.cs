using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

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
        private GameManager _gameManager;
        private Vector3 _spawnPosition;
        
        public Pizza pizzaDelivered { get; private set; }
        public Dictionary<Ingredient, int> order { get; private set; }
        
        private int _errorsMade;
        
        private void Start()
        {
            _rnd = new System.Random();
            _gameManager = FindFirstObjectByType<GameManager>();
            _spawnPosition = Vector3.zero + orderSpawnPos.position;
        }

        public int DeliverPizza(Pizza pizza, Dictionary<Ingredient, int> deliveredOrder)
        {
            pizzaDelivered = pizza;
            order = deliveredOrder;
            _errorsMade = 0;

            CheckOrderCorrect();
            return _errorsMade;
        }

        private async void CheckOrderCorrect()
        {
            await Task.Run(TaskCheckOrderCorrect);
        }

        private void TaskCheckOrderCorrect()
        {
            Dictionary<Ingredient, int> pizzaIngredients = new Dictionary<Ingredient, int>();
            foreach (Ingredient ingredient in pizzaDelivered.AttachedIngredients)
            {
                if(!pizzaIngredients.TryAdd(ingredient, 1)) pizzaIngredients[ingredient]++;
            }

            foreach (Ingredient ingredient in pizzaIngredients.Keys)
            {
                if (!order.TryGetValue(ingredient, out int amount))
                {
                    _errorsMade++;
                    continue;
                }
                
                if(amount != pizzaIngredients[ingredient]) _errorsMade += math.abs(amount - pizzaIngredients[ingredient]);
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
                _errorsMade += 10;
            }   
        }

        public async void GenerateOrder(int orderNumber)
        {
            await Task.Run(TaskGenerateOrder);
            
            var newOrder = Instantiate(orderPrefab, _spawnPosition, Quaternion.identity);
            newOrder.GetComponent<Order>().SetOrder(_order, orderNumber, _gameManager.GetPizzaValue(_order));
        }
        
        private void TaskGenerateOrder()
        {
            _availableIngredients = new List<Ingredient>(validIngredients);
            _order = new Dictionary<Ingredient, int>();
            int ingredientsNumber = _rnd.Next(1, maxIngredientNumber);

            for (int i = 0; i < ingredientsNumber; i++)
            {
                Ingredient newIngredient = _availableIngredients[_rnd.Next(0, _availableIngredients.Count)];
                int amount = newIngredient.SnapToPizza ? 1 : _rnd.Next(1, maxPerIngredientAmount);

                if(!_order.TryAdd(newIngredient, amount))
                {
                    _order[newIngredient] += amount;
                }

                if(newIngredient.SnapToPizza || !allowRepeatedIngredients) _availableIngredients.Remove(newIngredient);
            }
        }
    }
}