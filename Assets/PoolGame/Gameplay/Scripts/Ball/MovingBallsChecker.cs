using System;
using PoolGame.Core.Events;
using PoolGame.Gameplay.Shooting;
using UnityEngine;

namespace PoolGame.Gameplay.Ball
{
    public class MovingBallsChecker : MonoBehaviour
    {
        [SerializeField] private BallContainer ballContainer;
        [SerializeField] private float stopSpeedThreshold = 0.3f;
        
        [Header("Events")]
        [SerializeField] private VoidEventChannel shotTakenEvent;
        [SerializeField] private VoidEventChannel ballsStoppedMovingEvent;
        [SerializeField] private VoidEventChannel gameOverEvent;

        private bool _ballsInPlay;
        
        private void OnEnable()
        {
            shotTakenEvent?.Subscribe(OnShotTaken);
            gameOverEvent?.Subscribe(OnGameOver);
        }

        private void OnDisable()
        {
            shotTakenEvent?.Unsubscribe(OnShotTaken);
            gameOverEvent.Unsubscribe(OnGameOver);
        }
        
        private void FixedUpdate()
        {
            if (!_ballsInPlay)
                return;
            
            if (AnyBallAboveThreshold())
                return;

            ForceStopAllBalls();
            _ballsInPlay = false;
            ballsStoppedMovingEvent?.RaiseEvent();
        }

        private void OnShotTaken()
        {
            _ballsInPlay = true;
        }

        private bool AnyBallAboveThreshold()
        {
            if (ballContainer == null)
                return false;

            foreach (BallController ball in ballContainer.ActiveBalls)
            {
                if (ball != null && ball.IsMovingAboveSpeed(stopSpeedThreshold))
                {
                    return true;
                }
            }
            return false;
        }

        private void ForceStopAllBalls()
        {
            if (ballContainer == null)
                return;

            foreach (BallController ball in ballContainer.ActiveBalls)
            {
                ball?.ForceStop();
            }
        }
        
        private void OnGameOver()
        {
            ForceStopAllBalls();
        }
    }
}
