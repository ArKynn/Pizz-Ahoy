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
        private bool _isStoreOpen;
        private int _daysUntilQuota;
        private bool isQuotaDay => _daysUntilQuota <= 0;
        public int day {get; private set;}

        private void Start()
        {
            _orderManager = FindFirstObjectByType<OrderManager>();
            ResetQuotaDay();
        }
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
                    _orderManager.GenerateOrder();
                }
            }
        }
        private float _newOrderTimer;

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
            if(!_isStoreOpen) return;
            
            day++;
            newOrderTimer = 0;
            dayTimer = 0;
            _daysUntilQuota--;
            
            OpenStore();
        }
        
        private void OpenStore() => _isStoreOpen = true;
        private void CloseStore()
        {
            _isStoreOpen = false;
            if (isQuotaDay) _gameManager.QuotaCheck();
        }
        
        public void ResetQuotaDay() => _daysUntilQuota = daysBetweenQuotas;
    }
}