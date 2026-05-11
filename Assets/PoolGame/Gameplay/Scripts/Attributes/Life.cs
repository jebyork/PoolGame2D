using System;
using UnityEngine;
using Attribute = PoolGame.Game.Attribute.Attribute;

namespace PoolGame.Gameplay.Attributes
{
    public class Life : Attribute
    {
        const int NoLife = -1;

        [SerializeField] int startingLife = 3;
        
        [SerializeField] int maximumEverLife = 10;
        [SerializeField] int startingMaxLife = 3;
        
        public event Action OnNoLife;

        public int MaxLife => _currentMaxLife;
        int _currentMaxLife;

        public event Action<int> OnMaxLifeChanged;

        #region Lifecycle
        
        void Awake()
        {
            _currentMaxLife = Mathf.Clamp(
                startingMaxLife,
                NoLife,
                maximumEverLife);

            SetLife(startingLife);
        }
        
        void Update()
        {
            Logwin.Log("Life", AttributeValue, "Life");
        }
        
        #endregion

        public override void DecreaseAttribute(int amount)
        {
            SetLife(AttributeValue - amount);
        }

        public override void IncreaseAttribute(int amount)
        {
            SetLife(AttributeValue + amount);
        }

        public override void ResetAttribute()
        {
	        _currentMaxLife = Mathf.Clamp(
		        startingMaxLife,
		        NoLife,
		        maximumEverLife);
	        
            SetLife(startingLife);
        }

        public void AdjustMaxLife(int amount, bool adjustLife = false)
        {
	        _currentMaxLife = Mathf.Clamp(_currentMaxLife + amount, NoLife, maximumEverLife);
            int lifeCheck = Mathf.Clamp(AttributeValue, NoLife, _currentMaxLife);
            if (lifeCheck != AttributeValue)
            {
                SetLife(lifeCheck);
            }
            OnMaxLifeChanged?.Invoke(_currentMaxLife);

            if (adjustLife)
                SetLife(_currentMaxLife);
        }
        
        void SetLife(int value)
        {
            AttributeValue = Mathf.Clamp(value, NoLife, _currentMaxLife);
            
            if (AttributeValue == NoLife)
	            OnNoLife?.Invoke();
        }
    }
}

