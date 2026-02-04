using UnityEngine;

namespace PoolGame.Gameplay.Aim
{
    public class NullAimStrategy : IAimStrategy
    {
        public void Begin(Vector3 startWorld)
        {
            Debug.LogWarning("NullAimStrategy Begin");
        }
        public void End()
        {
            Debug.LogWarning("NullAimStrategy End");
        }
        public bool TryGetSnapshot(out AimSnapshot snapshot)
        {
            snapshot = default;
            return false;
        }
    }
}
