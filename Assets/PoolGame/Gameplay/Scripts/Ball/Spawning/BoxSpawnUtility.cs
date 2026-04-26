using System;
using System.Collections.Generic;
using PoolGame.Core.Helpers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PoolGame.Gameplay.Ball.Spawning
{
    public static class BoxSpawnUtility
    {
        public static bool TryFindSpawnPosition(
            BoxCollider2D boxCollider,
            float edgePadding,
            int maxAttempts,
            Predicate<Vector2> isValidPosition,
            out Vector2 spawnPosition)
        {
            if (boxCollider == null)
            {
                spawnPosition = Vector2.zero;
                return false;
            }

            int attemptLimit = Mathf.Max(1, maxAttempts);

            for (int attempt = 0; attempt < attemptLimit; attempt++)
            {
                Vector2 candidate = GetRandomPointInsideBox(boxCollider, edgePadding);
                if (isValidPosition != null && !isValidPosition(candidate))
                    continue;

                spawnPosition = candidate;
                return true;
            }

            spawnPosition = Vector2.zero;
            return false;
        }

        public static bool IsFarEnoughFromPositions(
            Vector2 candidate,
            float minCenterDistance,
            IReadOnlyList<Vector2> existingPositions)
        {
            float minCenterDistanceSquared = minCenterDistance * minCenterDistance;

            foreach (Vector2 existingPosition in existingPositions)
            {
                if ((candidate - existingPosition).sqrMagnitude < minCenterDistanceSquared)
                    return false;
            }

            return true;
        }

        public static bool TryGetPlacementRadius(
            GameObject prefab,
            Component context,
            string ownerName,
            out float radius)
        {
            if (prefab == null)
            {
                Debug.LogError($"[{ownerName}] prefab is null.", context);
                radius = 0f;
                return false;
            }

            if (prefab.TryGetComponent(out CircleCollider2D circleCollider))
            {
                radius = circleCollider.GetWorldCircleRadius();
                return radius > 0f;
            }

            if (prefab.TryGetComponent(out BoxCollider2D boxCollider))
            {
                Vector2 scale = boxCollider.transform.lossyScale;
                Vector2 halfSize = boxCollider.size * 0.5f;
                Vector2 scaledHalfSize = new(
                    halfSize.x * Mathf.Abs(scale.x),
                    halfSize.y * Mathf.Abs(scale.y));

                radius = scaledHalfSize.magnitude + boxCollider.edgeRadius;
                return radius > 0f;
            }

            Debug.LogError(
                $"[{ownerName}] {prefab.name} is missing a supported Collider2D for spawn spacing.",
                context);

            radius = 0f;
            return false;
        }

        private static Vector2 GetRandomPointInsideBox(BoxCollider2D boxCollider, float edgePadding)
        {
            Vector2 halfSize = boxCollider.size * 0.5f;
            Vector2 padding = Vector2.one * edgePadding;
            Vector2 min = boxCollider.offset - halfSize + padding;
            Vector2 max = boxCollider.offset + halfSize - padding;

            float x = Random.Range(min.x, max.x);
            float y = Random.Range(min.y, max.y);

            return boxCollider.transform.TransformPoint(new Vector2(x, y));
        }
    }
}