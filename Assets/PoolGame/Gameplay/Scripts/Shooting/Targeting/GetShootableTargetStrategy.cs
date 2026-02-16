using UnityEngine;

namespace PoolGame.Gameplay.Shooting.Targeting
{
    public abstract class GetShootableTargetStrategy : ScriptableObject
    {
        public abstract IShootable GetShootable();
    }
}
