using System;
using PoolGame.Gameplay.Ball;
using UnityEngine;
using UnityEngine.Events;

namespace PoolGame.Gameplay.GameMode
{
    public class PotAllGameMode : MonoBehaviour, ITurnOutcomeHandler
    {
        [Header("Components")]
        [SerializeField] private BallContainer ballContainer;
        [SerializeField] private GameState gameState;
        [SerializeField] private MovingBallsChecker movingBallsChecker;

        [Header("Spawning")]
        public UnityEvent spawnObjectBallsCommand;
        public UnityEvent spawnCueBallCommand;

        private void OnEnable()
        {
            gameState.RegisterHandler(this);
        }

        private void OnDisable()
        {
            gameState.UnregisterHandler(this);
        }

        private void Start()
        {
            SpawnBalls();
        }

        public void OnTurnEvaluate(Action onComplete)
        {
            if (gameState.CurrentGameState != GameStateEnum.Finished)
                SpawnBalls();

            onComplete();
        }

        private void SpawnBalls()
        {
            if (ballContainer.GetActiveBallCount(BallType.CueBall) == 0)
                spawnCueBallCommand?.Invoke();

            if (ballContainer.GetActiveBallCount(BallType.ObjectBall) == 0)
                spawnObjectBallsCommand?.Invoke();
        }

        public bool CanTakePlayerShot()
        {
            if (gameState == null || gameState.CurrentGameState == GameStateEnum.Finished)
                return false;

            if (gameState.CurrentGameState == GameStateEnum.AwaitingTurn)
                return true;

            if (gameState.CurrentGameState != GameStateEnum.TurnInProgress)
                return false;

            return movingBallsChecker != null && movingBallsChecker.CanTakeShot();
        }

        public void OnNoLifeLeft()
        {
            gameState.SetGameOver();
            ballContainer.ReleaseAll();
        }
    }
}
