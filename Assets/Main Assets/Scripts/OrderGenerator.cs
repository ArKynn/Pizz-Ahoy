using System;
using System.Collections.Generic;
using UnityEngine;

namespace Main_Assets.Scripts
{
    public class OrderGenerator : MonoBehaviour
    {
        [SerializeField] private Ingredient[] validIngredients;
        [SerializeField] private int maxIngredientNumber;
        [SerializeField] private int maxPerIngredientAmount;
        
        private System.Random _rnd;
        private Dictionary<Ingredient, int> _order;

        private void Start()
        {
            _rnd = new System.Random();
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