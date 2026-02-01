using System;
using PoolGame.Core.Events.Channels;
using UnityEngine;

namespace PoolGame.Core.Game.States.Gameplay
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
