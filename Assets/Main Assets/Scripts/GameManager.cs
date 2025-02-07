using System;
using System.Collections.Generic;
using UnityEngine;

namespace Main_Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private float dayTimeInSeconds;
        [SerializeField] private float delayBetweenNewOrders;
        [SerializeField] private float errorPaymentReduction;
        private OrderChecker orderChecker;
        private OrderGenerator orderGenerator;
        private bool _isStoreOpen;
        public int Money {get; private set;}
        public int ProfitSinceLastCheck {get; private set;}

        private void Start()
        {
            orderChecker = FindFirstObjectByType<OrderChecker>();
            orderGenerator = FindFirstObjectByType<OrderGenerator>();
        }

        private void Update()
        {
            if(!_isStoreOpen) return;
        }
        

        public void DeliverPizza(Pizza pizza, Dictionary<Ingredient, int> order)
        {
            var errors = orderChecker.DeliverPizza(pizza, order);

            GetDeliveryPayment(errors, GetPizzaValue(order));
        }
        
        private int GetPizzaValue(Dictionary<Ingredient, int> order)
        {
            int value = 0;
            foreach (Ingredient ingredient in order.Keys )
            {
                //value += ingredient.MoneyValue;
            }
            
            return value;
        }
        
        private void GetDeliveryPayment(int errors, int pizzaValue)
        {
            var reduction = errors * errorPaymentReduction;
            var profit = Mathf.RoundToInt(pizzaValue * reduction);
            
            Money += profit;
            ProfitSinceLastCheck += profit;
        }
    }
    
}