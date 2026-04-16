using PoolGame.Core.Events;
using PoolGame.Gameplay.Attributes;
using PoolGame.Gameplay.Ball;
using PoolGame.Gameplay.Table.Pockets;
using UnityEngine;

namespace PoolGame.Gameplay.GameMode
{
    public class PotAllGameMode : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private VoidEventChannel spawnObjectBallsCommand;
        [SerializeField] private VoidEventChannel spawnCueBallCommand;
        [SerializeField] private VoidEventChannel shotTakenEvent;
        [SerializeField] private VoidEventChannel ballsStoppedEvent;
        [SerializeField] private BallPocketedChannel ballPocketedEvent;
        [SerializeField] private VoidEventChannel gameOverCommand;
        
        [Header("Components")]
        [SerializeField] private BallContainer ballContainer;

        [Header("Attributes")]
        [SerializeField] private Life life;
        [SerializeField] private Score score;
        
        
        private bool _cueBallPottedThisShot;
        private bool _ballsInPlay;
        private bool _gameOver = false;
        
        private void OnEnable()
        {
            ballPocketedEvent?.Subscribe(BallPocketed);
            ballsStoppedEvent?.Subscribe(BallsStoppedMoving);
            shotTakenEvent?.Subscribe(ShotTaken);
        }

        private void OnDisable()
        {
            ballPocketedEvent?.Unsubscribe(BallPocketed);
            ballsStoppedEvent?.Unsubscribe(BallsStoppedMoving);
            shotTakenEvent?.Unsubscribe(ShotTaken);
        }
        
        private void Start()
        {
            _cueBallPottedThisShot = false;
            spawnObjectBallsCommand.RaiseEvent();
            spawnCueBallCommand.RaiseEvent();
        }
        
        private void BallPocketed(BallPocketedEvent evt)
        {
            if (!_ballsInPlay)
                return;
            
            if (evt.PottedBall == null)
                return;

            if (evt.PottedBall.BallType == BallType.CueBall)
            {
                _cueBallPottedThisShot = true;
            }
            
            ballContainer.ReleaseBall(evt.PottedBall);
        }

        public bool CanTakePlayerShot() => !_ballsInPlay && !_gameOver;

        
        private void BallsStoppedMoving()
        {
            if (_cueBallPottedThisShot)
            {
                spawnCueBallCommand.RaiseEvent();
            }

            if (ballContainer.GetActiveBallCount(BallType.ObjectBall) == 0)
            {
                spawnObjectBallsCommand.RaiseEvent();
            }
            
            _ballsInPlay = false;
            _cueBallPottedThisShot = false;
        }
        
        private void ShotTaken()
        {
            _ballsInPlay = true;
        }

        public void OnNoLifeLeft()
        {
            Debug.Log("No life left");
            _gameOver = true;
            gameOverCommand?.RaiseEvent();
        }
    }
}
