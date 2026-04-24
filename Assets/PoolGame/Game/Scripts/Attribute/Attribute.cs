using System;
using UnityEngine;

namespace PoolGame.Game.Attribute
{
    public abstract class Attribute : MonoBehaviour
    {
        public Action<int> OnAttributeChanged;

        private int _attributeValue;
        protected int AttributeValue
        {
            get => _attributeValue;
            set
            {
                _attributeValue = value;
                OnAttributeChanged?.Invoke(value);
            }
        }
        
        public abstract void DecreaseAttribute(int amount);
        public abstract void IncreaseAttribute(int amount);
        
        public int GetAttributeValue() => _attributeValue;
        
    }
}