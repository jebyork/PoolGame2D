using UnityEngine;

namespace PoolGame.Gameplay.Shooting.Aiming
{
    [CreateAssetMenu(fileName = "Calculate Aim From Shootable", menuName = "Shooting/Aiming/Calculate Aim From Shootable")]
    public class CalculateAimFromShootable : CalculateAimDataStrategy
    {
        [Min(0.001f)] [SerializeField] private float maxPullDistance = 1.5f;
        
        protected override Vector3 CalculateDirection(AimingCalculationData aimingCalculationData)
        {
            return aimingCalculationData.Shootable.GetPosition() - aimingCalculationData.CurrentMousePos;
        }

        protected override float CalculatePower()
        {
            float pullDistance = Direction.magnitude;
            return Mathf.Clamp01(pullDistance / maxPullDistance);
        }
    }
}
