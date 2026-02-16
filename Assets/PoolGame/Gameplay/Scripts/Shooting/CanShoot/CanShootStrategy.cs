using PoolGame.Gameplay.Shooting.Aiming;
using UnityEngine;

namespace PoolGame.Gameplay.Shooting.CanShoot
{
    public abstract class CanShootStrategy : ScriptableObject
    {
        [SerializeField] private AimingCalculationDataObserver aimingCalculationDataObserver;
        [SerializeField] private AimingDataObserver aimingDataObserver;

        public bool CanShoot()
        {
            return CanShootImplementation(aimingCalculationDataObserver.Value, aimingDataObserver.Value);    
        }
        
        protected abstract bool CanShootImplementation(AimingCalculationData calculationData, AimingData aimingData);
    }
}