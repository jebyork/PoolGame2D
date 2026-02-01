using UnityEngine;

namespace PoolGame.Core.Game.States.Gameplay.Shot
{
    public interface IShotTarget
    {
        public Vector3 AimPointWorld { get; }
        public void ApplyShot(Vector3 direction, float power01);
    }
}
