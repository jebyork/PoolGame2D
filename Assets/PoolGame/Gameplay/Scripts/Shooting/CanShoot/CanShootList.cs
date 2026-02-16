using System.Collections.Generic;
using PoolGame.Gameplay.Shooting.Aiming;
using UnityEngine;

namespace PoolGame.Gameplay.Shooting.CanShoot
{
    [CreateAssetMenu(fileName = "Can Shoot List", menuName = "Shooting/Can Shoot/List", order = 0)]
    public class CanShootList : CanShootStrategy
    {
        [SerializeField] private List<CanShootStrategy> canShootStrategyList;


        protected override bool CanShootImplementation(AimingCalculationData calculationData, AimingData aimingData)
        {
            foreach (CanShootStrategy strategy in canShootStrategyList)
            {
                if (strategy.CanShoot() == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}