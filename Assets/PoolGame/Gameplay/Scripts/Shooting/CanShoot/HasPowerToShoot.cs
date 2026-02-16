using PoolGame.Gameplay.Shooting.Aiming;
using UnityEngine;

namespace PoolGame.Gameplay.Shooting.CanShoot
{
    [CreateAssetMenu(fileName = "Shot Has Power", menuName = "Shooting/Can Shoot/HasPower", order = 0)]
    public class HasPowerToShoot : CanShootStrategy
    {
        [SerializeField, Range(0f, 1f)] private float minPower;
        
        protected override bool CanShootImplementation(AimingCalculationData calculationData, AimingData aimingData)
        {
            return aimingData.Power01 >= minPower; 
        }
    }
}