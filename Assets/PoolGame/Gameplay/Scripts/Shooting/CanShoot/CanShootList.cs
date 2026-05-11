using System.Collections.Generic;
using PoolGame.Gameplay.Shooting.Aiming;
using UnityEngine;

namespace PoolGame.Gameplay.Shooting.CanShoot
{
    [CreateAssetMenu(fileName = "Can Shoot List", menuName = "Shooting/Can Shoot/List", order = 0)]
    public class CanShootList : CanShootStrategy
    {
        [SerializeField] List<CanShootStrategy> canShootStrategyList;

        public override bool CanShoot(PlayerShootingController shotRequester)
        {
            foreach (CanShootStrategy strategy in canShootStrategyList)
            {
                if (strategy.CanShoot(shotRequester) == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}