using UnityEngine;

namespace PoolGame.Game.Scripts.ScreenToWorld
{
    public abstract class ScreenToWorldStrategy : ScriptableObject
    {
        public abstract Vector3 ScreenToWorld();
    }
}
