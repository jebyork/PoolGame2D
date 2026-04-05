using System;
using PoolGame.Core.Helpers;
using UnityEngine;

namespace PoolGame.Gameplay.Ball.Spawning
{
    public class TriangleBallSpawner : BallSpawner
    {
        [SerializeField] private int ballCount = 15;
        [SerializeField] private float rackGap = 0.0001f;
        [SerializeField] private float rackAngleDeg = 0f;
        [SerializeField] private int previewSegments = 20;

        private void ProcessPositions(Action<Vector2, float> callback)
        {
            if (!CanSpawn())
                return;

            float ballRadius = GetBallRadius();
            Vector2[] positions = CalculateRackPositions(ballRadius);

            foreach (Vector2 position in positions)
            {
                callback(position, ballRadius);
            }
        }
        
        protected override void Spawn()
        {
            ProcessPositions((pos, radius) => 
                SpawnBall(new Vector3(pos.x, pos.y, 0f)));
        }

        private void OnDrawGizmosSelected()
        {
            ProcessPositions((pos, radius) =>
                CircleHelpers.DrawCircle(
                    pos, 
                    radius, 
                    previewSegments, 
                    Color.red)
                );
        }

        private bool CanSpawn()
        {
            return ballCount > 0 && ballPrefab != null;
        }

        private float GetBallRadius()
        {
            CircleCollider2D circle = ballPrefab.GetComponent<CircleCollider2D>();
            if (circle == null)
            {
                Debug.LogError("[TriangleBallSpawner] ballPrefab is missing CircleCollider2D");
                return 0f;
            }

            return circle.GetWorldCircleRadius();
        }
        
        private Vector2[] CalculateRackPositions(float ballRadius)
        {
            if (!HasValidRackConfiguration(ballRadius))
                return Array.Empty<Vector2>();

            Vector2[] positions = new Vector2[ballCount];

            float spacing = GetBallSpacing(ballRadius);
            float rowSpacing = GetRowSpacing(spacing);

            Vector2 origin = transform.position;
            Vector2 forward = GetForwardDirection();
            Vector2 sideways = GetSidewaysDirection(forward);

            FillRackPositions(positions, origin, forward, sideways, spacing, rowSpacing);

            return positions;
        }

        private bool HasValidRackConfiguration(float ballRadius)
        {
            return ballCount > 0 && ballRadius > 0f;
        }

        private float GetBallSpacing(float ballRadius)
        {
            return ballRadius * 2f + rackGap;
        }

        private float GetRowSpacing(float ballSpacing)
        {
            // Row spacing for triangular packing
            return Mathf.Sqrt(3f) * 0.5f * ballSpacing;
        }

        private Vector2 GetForwardDirection()
        {
            float angleRadians = rackAngleDeg * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians)).normalized;
        }

        private Vector2 GetSidewaysDirection(Vector2 forward)
        {
            return new Vector2(-forward.y, forward.x);
        }

        private void FillRackPositions(Vector2[] positions, Vector2 origin, Vector2 forward,
            Vector2 sideways, float spacing, float rowSpacing)
        {
            int index = 0;

            for (int row = 0; index < positions.Length; row++)
            {
                int ballsInRow = GetBallsInRow(row);
                Vector2 rowCenter = GetRowCenter(origin, forward, row, rowSpacing);
                float startOffset = GetRowStartOffset(ballsInRow, spacing);

                for (int column = 0; column < ballsInRow && index < positions.Length; column++)
                {
                    positions[index] = GetBallPosition(rowCenter, sideways, startOffset, column, spacing);
                    index++;
                }
            }
        }

        private int GetBallsInRow(int row)
        {
            return row + 1;
        }

        private Vector2 GetRowCenter(Vector2 origin, Vector2 forward, int row, float rowSpacing)
        {
            return origin + forward * (row * rowSpacing);
        }

        private float GetRowStartOffset(int ballsInRow, float spacing)
        {
            float rowWidth = (ballsInRow - 1) * spacing;
            return -rowWidth * 0.5f;
        }

        private Vector2 GetBallPosition(
            Vector2 rowCenter,
            Vector2 sideways,
            float startOffset,
            int column,
            float spacing)
        {
            float offset = startOffset + column * spacing;
            return rowCenter + sideways * offset;
        }
    }
}