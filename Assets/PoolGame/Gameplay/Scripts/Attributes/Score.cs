using PoolGame.Gameplay.Table.Pockets;
using UnityEngine;

namespace PoolGame.Gameplay.Attributes
{
    public class Score : MonoBehaviour, IAttribute
    {
        [Header("Events")]
        [SerializeField] private BallPocketedChannel ballPocketedEvent;
        
        [Header("Score")]
        [SerializeField] private int scoreIncreaseBase = 1;
        [SerializeField] private int scoreDecreaseBase = 3;
        
        private static readonly int NoScore = 0;
        private int _currentScore = NoScore;
        
        private void OnEnable()
        {
            ballPocketedEvent?.Subscribe(BallPocketed);
        }
        
        private void OnDisable()
        {
            ballPocketedEvent?.Unsubscribe(BallPocketed);
        }
        
        public void DecreaseAttribute(int amount)
        {
            _currentScore = Mathf.Min(_currentScore - amount, NoScore);
        }

        public void IncreaseAttribute(int amount)
        {
            _currentScore += amount;
        }

        public int GetAttributeValue()
        {
            return _currentScore;
        }

        private void BallPocketed(BallPocketedEvent evt)
        {
            if (evt.PottedBall == null)
                return;
            
            switch (evt.PottedBall.BallType)
            {
                case BallType.ObjectBall:
                    IncreaseAttribute(scoreIncreaseBase);
                    break;

                case BallType.CueBall:
                    DecreaseAttribute(scoreIncreaseBase);
                    break;
            }
        }
    }
}