using PoolGame.Gameplay.Table.Pockets;
using UnityEngine;
using UnityEngine.Events;

namespace PoolGame.Gameplay.Attributes
{
    public class Life : MonoBehaviour, IAttribute
    {
        [SerializeField] private int startingLife = 3;
        [SerializeField] private int maxLife;
        [SerializeField] private int lifeDecreaseBase = 1;
        
        [Header("Events")]
        [SerializeField] private BallPocketedChannel ballPocketedEvent;

        private static readonly int NoLife = 0;
        private int _currentLife;
        
        public UnityEvent onNoLife;

        private void OnEnable()
        {
            ballPocketedEvent?.Subscribe(BallPocketed);
        }
        
        private void OnDisable()
        {
            ballPocketedEvent?.Unsubscribe(BallPocketed);
        }
        
        private void Start()
        {
            _currentLife = startingLife;
        }
        
        private void BallPocketed(BallPocketedEvent evt)
        {
            if (evt.PottedBall == null)
                return;

            if (evt.PottedBall.BallType != BallType.CueBall) 
                return;
            DecreaseAttribute(lifeDecreaseBase);
        }
        
        public void DecreaseAttribute(int amount)
        {
            _currentLife = Mathf.Clamp(_currentLife - amount, NoLife, maxLife);
            if (_currentLife <= NoLife)
            {
                onNoLife?.Invoke();
            }
        }

        public void IncreaseAttribute(int amount)
        {
            _currentLife = Mathf.Clamp(_currentLife + amount, NoLife, maxLife);
        }

        public int GetAttributeValue()
        {
            return _currentLife;
        }
    }
}
