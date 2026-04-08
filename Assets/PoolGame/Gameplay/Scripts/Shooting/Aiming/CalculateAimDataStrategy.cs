using UnityEngine;
using UnityEngine.Serialization;

namespace PoolGame.Gameplay.Shooting.Aiming
{
    public abstract class CalculateAimDataStrategy : ScriptableObject
    {
        protected Vector3 Direction;

        public AimingData CalculateAimData(AimingCalculationData aimingCalculationData)
        {
            Direction = CalculateDirection(aimingCalculationData);
            Direction.z = 0;
            float powerPercent = CalculatePower();
            return new AimingData(Direction.normalized , powerPercent);
        }
        
        protected abstract Vector3 CalculateDirection(AimingCalculationData aimingCalculationData);
        
        protected abstract float CalculatePower();

    }
}
