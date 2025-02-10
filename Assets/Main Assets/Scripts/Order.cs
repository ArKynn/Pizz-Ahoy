using System.Collections.Generic;
using UnityEngine;

namespace Main_Assets.Scripts
{
    public class Order : MonoBehaviour
    {
        public Dictionary<Ingredient, int> order { get; private set; } = new Dictionary<Ingredient, int>();
        private bool orderSet;
        
        public void SetOrder(Dictionary<Ingredient, int> newOrder)
        {
            if(orderSet) return;
            
            order = newOrder;
            orderSet = true;
        }
    }
}