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

        private bool _ballsInPlay;
        
        private void OnEnable()
        {
            if (shotTakenEvent != null)
            {
                shotTakenEvent.Subscribe(OnShotTaken);
            }
        }

        private void OnDisable()
        {
            if (shotTakenEvent != null)
            {
                shotTakenEvent.Unsubscribe(OnShotTaken);
            }
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
    }
}
