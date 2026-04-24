using System;
using UnityEngine;
using UnityEngine.Events;
using Attribute = PoolGame.Game.Attribute.Attribute;

namespace PoolGame.Gameplay.Attributes
{
    public class Life : Attribute
    {
        private const int NoLife = 0;

        [SerializeField] private int startingLife = 3;
        [SerializeField] private int currentMaxLife;
        [SerializeField] private int maximumEverLife = 10;
        [SerializeField] private UnityEvent onNoLife;

        public int MaxLife => currentMaxLife;

        public event Action<int> OnMaxLifeChanged;

        private void Update()
        {
            Logwin.Log("Life", AttributeValue, "Life");
        }

        private void Awake()
        {
            currentMaxLife = Mathf.Clamp(
                currentMaxLife > NoLife ? currentMaxLife : startingLife,
                NoLife,
                maximumEverLife);

            SetLife(startingLife);
        }

        public override void DecreaseAttribute(int amount)
        {
            SetLife(AttributeValue - amount);
        }

        public override void IncreaseAttribute(int amount)
        {
            SetLife(AttributeValue + amount);
        }

        public void AdjustMaxLife(int amount, bool adjustLife = false)
        {
            currentMaxLife = Mathf.Clamp(currentMaxLife + amount, NoLife, maximumEverLife);
            int lifeCheck = Mathf.Clamp(AttributeValue, NoLife, currentMaxLife);
            if (lifeCheck != AttributeValue)
            {
                SetLife(lifeCheck);
            }
            OnMaxLifeChanged?.Invoke(currentMaxLife);

            if (adjustLife)
                SetLife(currentMaxLife);
        }
        
        private void SetLife(int value)
        {
            AttributeValue = Mathf.Clamp(value, NoLife, currentMaxLife);
            
            if (AttributeValue == NoLife)
                onNoLife?.Invoke();
        }
    }
}

