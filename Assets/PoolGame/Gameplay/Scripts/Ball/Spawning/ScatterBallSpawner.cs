using System.Collections.Generic;
using PoolGame.Core.Helpers;
using UnityEngine;

namespace PoolGame.Gameplay.Ball.Spawning
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ScatterBallSpawner : BallSpawner
    {
        [SerializeField] private int ballCount = 15;
        [SerializeField] private float gapBetweenBalls = 0.5f;
        [SerializeField] private int maxAttemptsPerBall = 50;
        private BoxCollider2D _boxCollider;

        #region Lifecycle

        private void Reset()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
        }

        private void Awake()
        {
            if (_boxCollider == null)
            {
                _boxCollider = GetComponent<BoxCollider2D>();
            }
        }
        
        #endregion


        public override void Spawn()
        {
            if (!CanSpawn())
                return;

            float ballRadius = GetBallRadius();
            if (ballRadius <= 0f)
                return;

            List<Vector2> spawnPositions = GenerateSpawnPositions(ballRadius);
            foreach (Vector2 spawnPosition in spawnPositions)
            {
                SpawnBall(spawnPosition);
            }
        }

        private bool CanSpawn()
        {
            if (ballCount <= 0)
                return false;

            if (ballPrefab == null)
            {
                Debug.LogError("[ScatterBallSpawner] ballPrefab is null.", this);
                return false;
            }

            if (_boxCollider != null) return true;
            Debug.LogError("[ScatterBallSpawner] BoxCollider2D reference is missing.", this);
            return false;

        }

        private float GetBallRadius()
        {
            return BoxSpawnUtility.TryGetPlacementRadius(
                ballPrefab,
                this,
                nameof(ScatterBallSpawner),
                out float radius) ? radius : 0f;
        }

        private List<Vector2> GenerateSpawnPositions(float ballRadius)
        {
            List<Vector2> positions = new(ballCount);
            float minCenterDistance = ballRadius * 2f + gapBetweenBalls;

            for (int i = 0; i < ballCount; i++)
            {
                bool foundPosition = BoxSpawnUtility.TryFindSpawnPosition(
                    _boxCollider,
                    ballRadius,
                    maxAttemptsPerBall,
                    candidate => IsValidSpawnPosition(candidate, minCenterDistance, positions),
                    out Vector2 spawnPosition);

                if (!foundPosition)
                {
                    LogInsufficientSpawnPositions(positions.Count);
                    break;
                }

                positions.Add(spawnPosition);
            }

            return positions;
        }
        
        private bool IsValidSpawnPosition(Vector2 candidate, float minCenterDistance, List<Vector2> positions)
        {
            return BoxSpawnUtility.IsFarEnoughFromPositions(candidate, minCenterDistance, positions);
        }

        private void LogInsufficientSpawnPositions(int validPositionCount)
        {
            Debug.LogWarning(
                $"[ScatterBallSpawner] Only found {validPositionCount} valid spawn positions out of {ballCount}. " +
                "Increase the box size or reduce the ball count / gap.",
                this);
        }
    }
}
