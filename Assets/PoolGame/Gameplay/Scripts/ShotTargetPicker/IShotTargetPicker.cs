using PoolGame.Gameplay.Shot;
using UnityEngine;

namespace PoolGame.Gameplay.ShotTargetPicker
{
    public interface IShotTargetPicker
    {
        ShotTargetPickResult TryPick();
    }
    
    public struct ShotTargetPickResult
    {
        public readonly IShootable Target;
        public readonly Vector3 HitPoint;
        public readonly bool HasHit;

        public ShotTargetPickResult(IShootable target , Vector3 hitPoint , bool hasHit)
        {
            Target = target;
            HitPoint = hitPoint;
            HasHit = hasHit;
        }
    }
    
}