using UnityEngine;
using UnityEngine.Serialization;

namespace PoolGame.Gameplay.Shooting.Aiming
{
    public abstract class CalculateAimDataStrategy : ScriptableObject
    {

        [Min(0.001f)] [SerializeField] private float maxPullDistance = 1.5f;

        public AimingData CalculateAimData(AimingCalculationData aimingCalculationData)
        {
            Vector3 direction = CalculateDirection(aimingCalculationData);
            direction.z = 0;
            float pullDistance = direction.magnitude;
            float powerPercent = Mathf.Clamp01(pullDistance / maxPullDistance);
            
            Vector3 normalizedDirection =
                pullDistance > 0f ? direction.normalized : Vector3.zero;
            
            Logwin.Log("Power", powerPercent);

            return new AimingData(normalizedDirection , powerPercent);
        }
        
        protected abstract Vector3 CalculateDirection(AimingCalculationData aimingCalculationData);

    }
}
