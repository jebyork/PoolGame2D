using PoolGame.Gameplay.Shooting.Aiming;
using UnityEngine;

namespace PoolGame.Gameplay.Shooting
{
    public interface IShootable
    {
        public Vector3 GetPosition();
        public void Shoot(AimingData aimingData);
    }
}
