using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main_Assets.Scripts
{
    public class Order : MonoBehaviour
    {
        [SerializeField] private GameObject orderIngredientPrefab;
        private GridLayoutGroup _grid;
        public Dictionary<Ingredient, int> order { get; private set; } = new Dictionary<Ingredient, int>();
        private bool _orderSet;

        public void SetOrder(Dictionary<Ingredient, int> newOrder, int orderNumber)
        {
            _grid = GetComponentInChildren<GridLayoutGroup>();
            if(_orderSet) return;
            
            order = newOrder;
            _orderSet = true;
            UpdateCanvasGroup(orderNumber);
        }

        private void UpdateCanvasGroup(int orderNumber)
        {
            var orderTitle = _grid.GetComponentInChildren<TMP_Text>();
            orderTitle.text = orderTitle.text.Replace("%", orderNumber.ToString());
            
            foreach (KeyValuePair<Ingredient, int> pair in order)
            {
                GameObject orderIngredient = Instantiate(orderIngredientPrefab, _grid.transform);
                TMP_Text orderIngredientText = orderIngredient.GetComponentInChildren<TMP_Text>();
                SpriteRenderer orderIngredientSprite = orderIngredient.GetComponentInChildren<SpriteRenderer>();
                
                orderIngredientText.text = orderIngredientText.text.Replace("%", pair.Value.ToString());
                if(pair.Key.Sprite != null) orderIngredientSprite.sprite = pair.Key.Sprite;
                else print($"{pair.Key} sprite is missing");
            }
        }
    }
}