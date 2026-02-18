using PoolGame.Gameplay.Shooting.Aiming;
using UnityEngine;

namespace PoolGame.Gameplay.Shooting.CanShoot
{
    [CreateAssetMenu(fileName = "Has Shootable To Shoot", menuName = "Shooting/Can Shoot/HasShootable", order = 0)]
    public class HasShootableToShoot : CanShootStrategy
    {
        [SerializeField] private AimingCalculationDataObserver aimingCalculationDataObserver;

        public override bool CanShoot()
        {
            return aimingCalculationDataObserver.Value.Shootable != null;
        }
    }
}