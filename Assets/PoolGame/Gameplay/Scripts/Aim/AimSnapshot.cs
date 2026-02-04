using System;
using UnityEngine;

namespace PoolGame.Gameplay.Aim
{
    [Serializable]
    public struct AimSnapshot
    {
        public Vector3 AimingPoint;
        public Vector3 CursorWorldPosition;
        public Vector3 AimEndPoint;
        
        public Vector3 ShotDirection;
        public float PullDistance;
        public float ShotPower01;
        
        public bool IsValidShot;
    }
}
