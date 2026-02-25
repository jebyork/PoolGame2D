using PoolGame.Core.Events.Channels;
using UnityEngine;

namespace PoolGame.Gameplay.Ball.Events
{
    [CreateAssetMenu(fileName = "Ball State Change", menuName = "Balls/Events/State Change", order = 0)]
    public class BallsStateChangeChannel : AbstractEventChannel<BallState>
    {
        
    }
    
    [System.Serializable]
    public enum BallState{
        Moving,
        Stopped
    }
}