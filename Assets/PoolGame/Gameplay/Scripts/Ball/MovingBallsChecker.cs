using System;
using PoolGame.Gameplay.GameMode;
using PoolGame.Gameplay.Shooting;
using UnityEngine;

namespace PoolGame.Gameplay.Ball
{
    public class MovingBallsChecker : MonoBehaviour
    {
        [Header("Components")] 
        [SerializeField] private PlayerShootingController playerShootingController;
        [SerializeField] private BallContainer ballContainer;
        [SerializeField] private GameState gameState;
        
        [SerializeField] private float stopSpeedThreshold = 0.3f;
        
        
        public Action OnBallsStoppedMoving;
        
        private bool _ballsInPlay;
        
        private void OnEnable()
        {
            if (playerShootingController)
                playerShootingController.OnShotTaken += OnShotTaken;
            if(gameState) 
                gameState.onGameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            if (playerShootingController)
                playerShootingController.OnShotTaken -= OnShotTaken;
            if(gameState) 
                gameState.onGameStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(GameStateEnum state)
        {
            if (state != GameStateEnum.Finished)
                return;
            
            ForceStopAllBalls();
        }

        private void FixedUpdate()
        {
            if (!_ballsInPlay)
                return;
            
            if (AnyBallAboveThreshold())
                return;

            ForceStopAllBalls();
            _ballsInPlay = false;
            OnBallsStoppedMoving?.Invoke();
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
