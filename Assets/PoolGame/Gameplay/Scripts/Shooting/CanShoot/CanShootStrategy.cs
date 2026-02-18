using PoolGame.Gameplay.Shooting.Aiming;
using UnityEngine;

namespace PoolGame.Gameplay.Shooting.CanShoot
{
    public abstract class CanShootStrategy : ScriptableObject
    {
        public abstract bool CanShoot();
    }
}