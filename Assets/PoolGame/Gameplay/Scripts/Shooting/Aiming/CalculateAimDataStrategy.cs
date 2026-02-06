using UnityEngine;

namespace PoolGame.Gameplay.Shooting.Aiming
{
    public abstract class CalculateAimDataStrategy : ScriptableObject
    {
        [Min(0.001f)]
        [SerializeField] protected float MaxPullDistance = 1.5f;

        public AimingData CalculateAimData(AimingCalculationData aimingCalculationData)
        {
            Vector3 direction = CalculateDirection(aimingCalculationData);
            direction.z = 0;
            float pullDistance  = direction.magnitude;
            float powerPercent = Mathf.Clamp01(pullDistance / MaxPullDistance);

            Vector3 normalizedDirection =
                pullDistance > 0f ? direction.normalized : Vector3.zero;

            return new AimingData(normalizedDirection , powerPercent);
        }
        
        protected abstract Vector3 CalculateDirection(AimingCalculationData aimingCalculationData);

    }
}
