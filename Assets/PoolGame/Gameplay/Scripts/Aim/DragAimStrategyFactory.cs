using PoolGame.Game.ScreenToWorld;
using UnityEngine;

namespace PoolGame.Gameplay.Aim
{
    [CreateAssetMenu(fileName = "Drag Aim Strategy" , menuName = "Aim Strategy/Drag Aim")]
    public sealed class DragAimStrategyFactory : AimStrategyFactory
    {
        [Min(0f)] 
        [SerializeField] private float minPower01 = 0.1f;

        [Min(0.001f)]
        [SerializeField] private float maxPullDistance = 1.5f;

        [SerializeField] private CameraScreenToWorldStrategy cursorWorldProvider;
        
        protected override IAimStrategy InternalCreateAimStrategy()
        {
            if (cursorWorldProvider == null) return null;

            return new DragAimStrategy(cursorWorldProvider , maxPullDistance , minPower01);
        }
    }
}