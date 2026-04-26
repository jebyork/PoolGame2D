using System.Collections.Generic;
using UnityEngine;

namespace PoolGame.Gameplay.Ball.Spawning
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ScatterBallSpawner : BallSpawner
    {
        [SerializeField] private int ballCount = 15;
        [SerializeField] private float gapBetweenBalls = 0.5f;
        [SerializeField] private int maxAttemptsPerBall = 50;
        [SerializeField] private BoxCollider2D boxCollider;

        private void Reset()
        {
            boxCollider = GetComponent<BoxCollider2D>();
        }

        private void Awake()
        {
            if (boxCollider == null)
            {
                boxCollider = GetComponent<BoxCollider2D>();
            }
        }

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

            if (boxCollider != null) return true;
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
                if (!BoxSpawnUtility.TryFindSpawnPosition(
                        boxCollider,
                        ballRadius,
                        maxAttemptsPerBall,
                        candidate => BoxSpawnUtility.IsFarEnoughFromPositions(
                            candidate,
                            minCenterDistance,
                            positions),
                        out Vector2 spawnPosition))
                {
                    Debug.LogWarning(
                        $"[ScatterBallSpawner] Only found {positions.Count} valid spawn positions out of {ballCount}. " +
                        "Increase the box size or reduce the ball count / gap.",
                        this);
                    break;
                }
                positions.Add(spawnPosition);
            }

            return positions;
        }
    }
}
