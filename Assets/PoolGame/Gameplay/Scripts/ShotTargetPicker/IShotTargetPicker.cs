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
        public IShootable Target;
        public Vector3 HitPoint;
        public bool HasHit;

        public ShotTargetPickResult(IShootable target , Vector3 hitPoint , bool hasHit)
        {
            Target = target;
            HitPoint = hitPoint;
            HasHit = hasHit;
        }
    }
    
}