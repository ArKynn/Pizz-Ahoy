using System;
using System.Collections.Generic;
using UnityEngine;

namespace Main_Assets.Scripts
{
    public class DayManager : MonoBehaviour
    {
        [SerializeField] private int daysBetweenQuotas;
        [SerializeField] private float dayTimeInSeconds;
        [SerializeField] private float delayBetweenNewOrders;
        private OrderManager _orderManager;
        private GameManager _gameManager;
        private UIManager _uiManager;
        private bool _isStoreOpen;
        private bool isQuotaDay => day % daysBetweenQuotas == 0;
        public int day {get; private set;}
        public int nextQuotaDay {get; private set;}
        private int _ordersGenerated;

        private float dayTimer
        {
            get => _dayTimer;
            set
            {
                _dayTimer = value;
                if (_dayTimer >= dayTimeInSeconds) CloseStore();
            }
        }
        private float _dayTimer;

        private float newOrderTimer
        {
            get => _newOrderTimer;
            set
            {
                _newOrderTimer = value;
                if (_newOrderTimer >= delayBetweenNewOrders)
                {
                    _ordersGenerated++;
                    _orderManager.GenerateOrder(_ordersGenerated);
                    _newOrderTimer = 0;
                }
            }
        }
        private float _newOrderTimer;

        private void Start()
        {
            _orderManager = FindFirstObjectByType<OrderManager>();
            _gameManager = FindFirstObjectByType<GameManager>();
            _uiManager = FindFirstObjectByType<UIManager>();
            nextQuotaDay = daysBetweenQuotas;
        }
        private void Update()
        {
            if(!_isStoreOpen) return;
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            dayTimer += Time.deltaTime;
            newOrderTimer += Time.deltaTime;
        }

        public void StartNewDay()
        {
            if(_isStoreOpen) return;
            
            print("Opening store");
            
            day++;
            newOrderTimer = delayBetweenNewOrders - 5;
            dayTimer = 0;
            _ordersGenerated = 0;
            
            OpenStore();
        }

        private void OpenStore()
        {
            _isStoreOpen = true;
            //_uiManager.StartCoroutine(FadeOutUI());
        }
        private void CloseStore()
        {
            print("Closing store");
            
            _isStoreOpen = false;
            //_uiManager.StartCoroutine(FadeInUI());
            if (isQuotaDay)
            {
                _gameManager.QuotaCheck();
                UpdateQuotaDay();
            }
        }

        private void UpdateQuotaDay()
        {
            nextQuotaDay = day + daysBetweenQuotas;
        }
    }
}