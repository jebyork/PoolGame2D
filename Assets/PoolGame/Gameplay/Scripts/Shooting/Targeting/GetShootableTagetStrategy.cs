using UnityEngine;

namespace PoolGame.Gameplay.Shooting.Targeting
{
    public abstract class GetShootableTagetStrategy : ScriptableObject
    {
        public abstract IShootable GetShootable();
    }
}
