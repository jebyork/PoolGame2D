using System;
using PoolGame.Core.Events.Channels;
using UnityEngine;

namespace PoolGame.Core.Game.States.Gameplay.Shot
{
    [CreateAssetMenu(fileName = "Shot Requested Channel", menuName = "Events/Shot Requested Channel")]
    public class ShotRequestedChannel : AbstractEventChannel<ShotData> {}
    
    [Serializable]
    public struct ShotData
    {
        public IShotTarget ShotTarget;
        
        public Vector3 ShotDirection;

        public float ShotPower01;
    }
}
