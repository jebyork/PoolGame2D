using PoolGame.Core.Helpers;
using UnityEngine;

namespace PoolGame.Gameplay.Guides
{
    public class BallHitGuideResponse : MonoBehaviour, IGuideHitResponse
    {
        [SerializeField] private CircleCollider2D circle;

        public GuideLineVisualData Resolve(RaycastHit2D hit , Vector2 incomingDir , float cueRadius , float maxLen)
        {
            if (circle == null) return default;
            
            Vector2 center = circle.transform.TransformPoint(circle.offset);
            float radius = circle.GetWorldCircleRadius();
            
            Vector2 dir = (-hit.normal).normalized;
            Vector2 start = center + dir * radius;
            Vector2 end = start + dir * maxLen;

            return new GuideLineVisualData(start , end);
        }
    }
}
