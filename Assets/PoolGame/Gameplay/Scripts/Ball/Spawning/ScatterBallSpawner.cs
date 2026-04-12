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
            CircleCollider2D circle = ballPrefab.GetComponent<CircleCollider2D>();
            if (circle != null) 
                return circle.GetWorldCircleRadius();
            Debug.LogError("[ScatterBallSpawner] ballPrefab is missing CircleCollider2D.", this);
            return 0f;

        }

        private List<Vector2> GenerateSpawnPositions(float ballRadius)
        {
            List<Vector2> positions = new(ballCount);
            float minCenterDistance = ballRadius * 2f + gapBetweenBalls;

            for (int i = 0; i < ballCount; i++)
            {
                if (!TryFindSpawnPosition(ballRadius, minCenterDistance, positions, out Vector2 spawnPosition))
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

        private bool TryFindSpawnPosition(
            float ballRadius,
            float minCenterDistance,
            List<Vector2> existingPositions,
            out Vector2 spawnPosition)
        {
            int attemptLimit = Mathf.Max(1, maxAttemptsPerBall);

            for (int attempt = 0; attempt < attemptLimit; attempt++)
            {
                Vector2 candidate = GetRandomPointInsideBox(ballRadius);
                if (!IsFarEnoughFromExisting(candidate, minCenterDistance, existingPositions)) continue;
                spawnPosition = candidate;
                return true;
            }

            spawnPosition = Vector2.zero;
            return false;
        }

        private Vector2 GetRandomPointInsideBox(float ballRadius)
        {
            Vector2 halfSize = boxCollider.size * 0.5f;
            Vector2 min = boxCollider.offset - halfSize + Vector2.one * ballRadius;
            Vector2 max = boxCollider.offset + halfSize - Vector2.one * ballRadius;

            float x = Random.Range(min.x, max.x);
            float y = Random.Range(min.y, max.y);

            return boxCollider.transform.TransformPoint(new Vector2(x, y));
        }

        private static bool IsFarEnoughFromExisting(
            Vector2 candidate,
            float minCenterDistance,
            List<Vector2> existingPositions)
        {
            float minCenterDistanceSquared = minCenterDistance * minCenterDistance;

            foreach (Vector2 existingPosition in existingPositions)
            {
                if ((candidate - existingPosition).sqrMagnitude < minCenterDistanceSquared)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
