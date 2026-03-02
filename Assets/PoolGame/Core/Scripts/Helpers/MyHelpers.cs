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
        
        public static Vector3 GetScreenToWorldPosition(Vector3 screenPosition)
        {
            if (Camera.main == null)
                return Vector3.zero;
            
            return Camera.main.ScreenToWorldPoint(screenPosition);
        }
    }
}
