using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main_Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private bool debugStartDay;
        [SerializeField] private int quotaBaseline;
        [SerializeField] private float quotaIncreaseModifier = 15;
        [SerializeField] private float errorPaymentReduction;
        [SerializeField] private int moneyGoal;
        public int money {get; private set;}
        public int profitSinceLastCheck {get; private set;}
        private OrderManager _orderManager;
        private DayManager _dayManager;
        private UIManager _uiManager;
        private Unity.Mathematics.Random _rnd;
        private int _quotasReached;
        private bool gameStarted = false;
        public int nextQuota {get; private set;}
        
        private void Start()
        {
            _rnd = new Unity.Mathematics.Random( 0x6E624EB7u);
            _dayManager = FindFirstObjectByType<DayManager>();
            _orderManager = FindFirstObjectByType<OrderManager>();
            _uiManager = FindFirstObjectByType<UIManager>();
            _quotasReached = 0;
            if(debugStartDay) StartGame();
        }

        public void StartGame()
        {
            if(gameStarted) return;
            GenerateNextQuota();
            _dayManager.StartNewDay();
            profitSinceLastCheck = 0;
            gameStarted = true;
        }
        
        public void DeliverPizza(Pizza pizza, Dictionary<Ingredient, int> order)
        {
            var errors = _orderManager.DeliverPizza(pizza, order);

            GetDeliveryPayment(errors, GetPizzaValue(order));
        }
        
        public int GetPizzaValue(Dictionary<Ingredient, int> order)
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

        private void GenerateNextQuota()
        {
            nextQuota = Mathf.RoundToInt(quotaBaseline * ((1 + Mathf.Pow(_quotasReached, 2) / quotaIncreaseModifier) * _rnd.NextFloat(0.75f, 1.25f) * _rnd.NextFloat(0.9f, 1.1f)));
            _uiManager.StartCoroutine(_uiManager.DisplayNewQuota(nextQuota, 3f));
        }

        public void QuotaCheck()
        {
            _uiManager.StartCoroutine(_uiManager.DisplayPayQuota(profitSinceLastCheck, nextQuota, 3f));
            
            if (profitSinceLastCheck < nextQuota)
            {
                GameOver();
                return;
            }

            if (money >= moneyGoal) WinGame();
            else
            {
                GenerateNextQuota();
            }
        }

        private void GameOver()
        {
            _uiManager.StartCoroutine(_uiManager.DisplayLoss(6f));
        }

        private void WinGame()
        {
            _uiManager.StartCoroutine(_uiManager.DisplayWin(6f));
        }
    }
}