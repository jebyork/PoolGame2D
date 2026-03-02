using UnityEngine;

namespace PoolGame.Core.Helpers
{
    public static class CircleHelpers
    {
        public static float GetWorldCircleRadius(this CircleCollider2D col)
        {
            Vector3 scale = col.transform.lossyScale;
            float maxScale = Mathf.Max(Mathf.Abs(scale.x), Mathf.Abs(scale.y));
            return col.radius * maxScale;
        }

        public static void DrawCircle(Vector2 center, float radius, int segments, Color color)
        {
            if (segments < 3)
            {
                segments = 3;
            }

            float step = 2f * Mathf.PI / segments;
            Vector2 prev = center + new Vector2(Mathf.Cos(0f), Mathf.Sin(0f)) * radius;

            for (int i = 1; i <= segments; i++)
            {
                float a = i * step;
                Vector2 next = center + new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * radius;
                Debug.DrawLine(prev, next, color);
                prev = next;
            }
        }

        /// <summary>
        /// Returns how much of the targetCircle's area is inside this collider.
        /// Result is between 0 and 1.
        /// </summary>
        public static float GetPercentageOfCircleInside(
            this CircleCollider2D containerCircle,
            CircleCollider2D targetCircle)
        {
            float containerRadius = containerCircle.GetWorldCircleRadius();
            float targetRadius = targetCircle.GetWorldCircleRadius();

            Vector2 containerCenter = containerCircle.transform.TransformPoint(containerCircle.offset);
            Vector2 targetCenter = targetCircle.transform.TransformPoint(targetCircle.offset);

            float intersectionArea = GetCircleIntersectionArea(
                containerCenter, containerRadius,
                targetCenter, targetRadius);

            float targetArea = GetCircleArea(targetRadius);

            return targetArea <= 0f ? 0f : Mathf.Clamp01(intersectionArea / targetArea);
        }

        private static float GetCircleArea(float radius)
        {
            return Mathf.PI * radius * radius;
        }

        /// <summary>
        /// Area of intersection between two circles
        /// </summary>
        private static float GetCircleIntersectionArea(
            Vector2 centerA, float radiusA,
            Vector2 centerB, float radiusB)
        {
            float distanceBetweenCenters = Vector2.Distance(centerA, centerB);

            // No overlap
            if (distanceBetweenCenters >= radiusA + radiusB)
                return 0f;

            // One circle fully inside the other
            if (distanceBetweenCenters <= Mathf.Abs(radiusA - radiusB))
            {
                float smallerRadius = Mathf.Min(radiusA, radiusB);
                return GetCircleArea(smallerRadius);
            }

            // Partial overlap
            float radiusASquared = radiusA * radiusA;
            float radiusBSquared = radiusB * radiusB;
            float distanceSquared = distanceBetweenCenters * distanceBetweenCenters;

            float angleA = Mathf.Acos(
                (distanceSquared + radiusASquared - radiusBSquared) /
                (2f * distanceBetweenCenters * radiusA)
            );

            float angleB = Mathf.Acos(
                (distanceSquared + radiusBSquared - radiusASquared) /
                (2f * distanceBetweenCenters * radiusB)
            );

            float overlapArea =
                radiusASquared * angleA +
                radiusBSquared * angleB -
                0.5f * Mathf.Sqrt(
                    (-distanceBetweenCenters + radiusA + radiusB) *
                    (distanceBetweenCenters + radiusA - radiusB) *
                    (distanceBetweenCenters - radiusA + radiusB) *
                    (distanceBetweenCenters + radiusA + radiusB)
                );

            return overlapArea;
        }
    }
}