using System;
using UnityEngine;

namespace PoolGame.Events
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
