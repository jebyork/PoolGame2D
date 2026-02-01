using UnityEngine;

namespace PoolGame.Core.Helpers
{
    public static class MyHelpers
    {
        public static bool IsNearlyZero(this Vector2 vec, float precision = 0.0001f)
        {
            return vec.sqrMagnitude < precision * precision;
        }
        
        public static bool IsNearlyZero(this Vector3 vec, float precision = 0.0001f)
        {
            return vec.sqrMagnitude < precision * precision;
        }
        
        public static bool IsNearlyZero(this float val, float precision = 0.0001f)
        {
            return Mathf.Abs(val) < precision;
        }
        
        public static bool ContainsLayer(this LayerMask mask, int layer)
        {
            return (mask.value & (1 << layer)) != 0;
        }
        
        public static float GetWorldCircleRadius(this CircleCollider2D col)
        {
            Vector3 s = col.transform.lossyScale;
            float maxScale = Mathf.Max(Mathf.Abs(s.x), Mathf.Abs(s.y));
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
    }
}
