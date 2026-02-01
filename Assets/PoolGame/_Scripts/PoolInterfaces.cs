using System;
using UnityEngine;

namespace PoolGame
{
    public interface IShotTarget
    {
        public Vector3 AimPointWorld { get; }
        public void ApplyShot(Vector3 direction, float power01);
    }
}
