using System;
using UnityEngine;

namespace PoolGame.Events
{
    [CreateAssetMenu(fileName = "Aim Update Chanel", menuName = "Events/Aim Update Chanel")]
    public class AimUpdatedChannel : AbstractEventChannel<AimSnapshot> {}
    
    [Serializable]
    public struct AimSnapshot
    {
        public Vector3 AimingPoint;
        public Vector3 CursorWorldPoint;
        public Vector3 ClampedEndAimingPoint;
        public Vector3 ShotDirection;

        public float ClampedPullDistance;
        public float ClampedPullPercentage;
    }
}
