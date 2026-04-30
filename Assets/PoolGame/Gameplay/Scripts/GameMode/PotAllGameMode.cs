using System;
using PoolGame.Core.Helpers;
using PoolGame.Gameplay.Attributes;
using PoolGame.Gameplay.Ball;
using PoolGame.Gameplay.GameMode.TurnEvaluation;
using PoolGame.Gameplay.Pickups;
using PoolGame.Gameplay.Shooting;
using UnityEngine;
using UnityEngine.Events;

namespace PoolGame.Gameplay.GameMode
{
    public class PotAllGameMode : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private Score score;
        [SerializeField] private Life life;
        [SerializeField] private Turn turn;
        [SerializeField] private BallContainer ballContainer;
        [SerializeField] private GameState gameState;
        [SerializeField] private TurnEvaluator turnEvaluator;
        [SerializeField] private PickupManager pickupManager;

        [Header("Spawning")]
        public UnityEvent spawnObjectBallsCommand;
        public UnityEvent spawnCueBallCommand;

        private void Awake()
        {
            if (!turnEvaluator)
                turnEvaluator = FindFirstObjectByType<TurnEvaluator>();

            if (!pickupManager)
                pickupManager = FindFirstObjectByType<PickupManager>();

            if (!gameState)
                gameState = FindFirstObjectByType<GameState>();

            if (!ballContainer)
                ballContainer = FindFirstObjectByType<BallContainer>();
        }

        public void StartGame()
        {
            ResetMode();
            ClearBalls();
            SpawnAllBalls();
        }
        
        private void ResetMode()
        {
            if (score)
                score.ResetAttribute();

            if (life)
                life.ResetAttribute();

            if (turn)
                turn.ResetAttribute();
        }

        private void ClearBalls()
        {
            if (ballContainer)
                ballContainer.ReleaseAll();
        }

        private void SpawnAllBalls()
        {
            spawnCueBallCommand?.Invoke();
            spawnObjectBallsCommand?.Invoke();
        }
        
        public void EvaluateTurn()
        {
            try
            {
                if (turnEvaluator)
                    turnEvaluator.EvaluateTurn();
                else
                    Debug.LogError($"[{gameObject.name}] PotAllGameMode: No TurnEvaluator assigned.", this);

                if (pickupManager && gameState)
                    pickupManager.EvaluateTurn(gameState.GetTurn());
            }
            finally
            {
                SpawnBallsIfNeeded();
            }
        }
        
        private void SpawnBallsIfNeeded()
        {
            if (ballContainer == null)
                return;

            // Spawn cue ball if missing
            if (ballContainer.GetActiveBallCount(BallType.CueBall) == 0)
                spawnCueBallCommand?.Invoke();

            // Spawn object balls if missing
            if (ballContainer.GetActiveBallCount(BallType.ObjectBall) == 0)
                spawnObjectBallsCommand?.Invoke();
        }

        public bool IsGameOver()
        {
            return life != null && life.GetAttributeValue() <= 0;
        }
    }
}
