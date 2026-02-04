using UnityEngine;

namespace PoolGame.Gameplay.Aim
{
    public interface IAimStrategy
    {
        void Begin(Vector3 startWorld);
        void End();
        
        bool TryGetSnapshot(out AimSnapshot snapshot);
    }
}
