using PoolGame.Gameplay.Shooting.Aiming;
using UnityEngine;

namespace PoolGame.Gameplay.Shooting.CanShoot
{
    [CreateAssetMenu(fileName = "Shot Has Power", menuName = "Shooting/Can Shoot/HasPower", order = 0)]
    public class HasPowerToShoot : CanShootStrategy
    {
        [SerializeField, Range(0f, 1f)] private float minPower;
        [SerializeField] private AimingDataObserver aimingDataObserver;

        public override bool CanShoot()
        {
            return aimingDataObserver.Value.Power01 >= minPower; 
        }
    }
}