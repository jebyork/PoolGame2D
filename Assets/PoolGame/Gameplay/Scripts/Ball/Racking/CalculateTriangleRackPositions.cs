using System;
using PoolGame.Core.Helpers;
using UnityEngine;

namespace PoolGame.Gameplay.Ball.Racking
{
    [CreateAssetMenu(fileName = "Triangle Rack" , menuName = "Balls/Racking/Triangle" , order = 0)]
    public class CalculateTriangleRackPositions : CalculateRackPositionsBase
    {
        public float RackGap = 0.0001f;
        public float RackAngleDeg = 0f;
        
        public override Vector2[] Calculate(int ballCount, float ballRadius)
        {
            if (ballCount <= 0 || ballRadius <= 0f) return Array.Empty<Vector2>();

            Vector2[] positions = new Vector2[ballCount];

            float diameter = ballRadius * 2f + RackGap;
            float rowStep = Mathf.Sqrt(3f) * 0.5f * diameter;

            float radians = RackAngleDeg * Mathf.Deg2Rad;
            Vector2 forward = new Vector2(Mathf.Sin(radians), Mathf.Cos(radians)).normalized;
            Vector2 right = new Vector2(forward.y, -forward.x);

            int ballIndex = 0;
            int rowIndex = 0;

            while (ballIndex < ballCount)
            {
                int ballsThisRow = rowIndex + 1;

                Vector2 rowOrigin = forward * (rowIndex * rowStep);
                float leftOffset = -0.5f * (ballsThisRow - 1) * diameter;

                for (int slotIndex = 0; slotIndex < ballsThisRow && ballIndex < ballCount; slotIndex++)
                {
                    float sideOffset = leftOffset + slotIndex * diameter;
                    positions[ballIndex] = rowOrigin + right * sideOffset;
                    ballIndex++;
                }

                rowIndex++;
            }

            return positions;
        }
    }
}
