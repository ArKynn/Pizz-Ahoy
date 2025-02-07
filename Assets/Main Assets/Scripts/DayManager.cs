using System;
using System.Collections.Generic;
using UnityEngine;

namespace Main_Assets.Scripts
{
    public class DayManager : MonoBehaviour
    {
        [SerializeField] private float daysBetweenQuotas;
        [SerializeField] private float dayTimeInSeconds;
        [SerializeField] private float delayBetweenNewOrders;
        [SerializeField] private float errorPaymentReduction;
        private OrderChecker orderChecker;
        private OrderGenerator orderGenerator;
        private bool _isStoreOpen;
        private int _ordersAvailable;
        private int _day;
        private float DayTimer
        {
            get => _dayTimer;
            set
            {
                _dayTimer = value;
                if (_dayTimer >= dayTimeInSeconds) CloseStore();
            }
        }
        private float _dayTimer;

        private float NewOrderTimer
        {
            get => _newOrderTimer;
            set
            {
                _newOrderTimer = value;
                if (_newOrderTimer >= delayBetweenNewOrders) GetNewOrder();
            }
        }
        private float _newOrderTimer;
        public int money {get; private set;}
        public int profitSinceLastCheck {get; private set;}

        private void Start()
        {
            orderChecker = FindFirstObjectByType<OrderChecker>();
            orderGenerator = FindFirstObjectByType<OrderGenerator>();
        }

        private void Update()
        {
            if(!_isStoreOpen) return;
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            DayTimer += Time.deltaTime;
            NewOrderTimer += Time.deltaTime;
        }

        private void StartNewDay()
        {
            _day++;
            NewOrderTimer = 0;
            DayTimer = 0;
        }
        
        public void OpenStore() => _isStoreOpen = true;

        private void CloseStore()
        {
            _isStoreOpen = false;
        }

        private void GetNewOrder()
        {
            orderGenerator.GenerateOrder();
            _ordersAvailable++;
        }
        
        public void DeliverPizza(Pizza pizza, Dictionary<Ingredient, int> order)
        {
            var errors = orderChecker.DeliverPizza(pizza, order);

            GetDeliveryPayment(errors, GetPizzaValue(order));

            _ordersAvailable--;
        }
        
        private int GetPizzaValue(Dictionary<Ingredient, int> order)
        {
            int value = 0;
            foreach (Ingredient ingredient in order.Keys )
            {
                value += ingredient.MoneyValue;
            }
            
            return value;
        }
        
        private void GetDeliveryPayment(int errors, int pizzaValue)
        {
            var reduction = errors * errorPaymentReduction;
            var profit = Mathf.RoundToInt(pizzaValue * reduction);
            
            money += profit;
            profitSinceLastCheck += profit;
        }
    }
    
}