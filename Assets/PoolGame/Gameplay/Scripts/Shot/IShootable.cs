using UnityEngine;

namespace PoolGame.Gameplay.Shot
{
    public interface IShootable
    {
        public void ApplyShot(Vector3 direction, float power01);
    }
}
