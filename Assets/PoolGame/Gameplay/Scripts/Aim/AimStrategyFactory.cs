using UnityEngine;

namespace PoolGame.Gameplay.Aim
{
    public abstract class AimStrategyFactory : ScriptableObject
    {
        protected abstract IAimStrategy InternalCreateAimStrategy();
        
        public IAimStrategy CreateAimStrategy()
        {
            return InternalCreateAimStrategy() ?? new NullAimStrategy();
        }
    }
}
