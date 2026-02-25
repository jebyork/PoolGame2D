using PoolGame.Core.Events.Channels;
using PoolGame.Gameplay.Ball;
using UnityEngine;

namespace PoolGame.Gameplay.Table.Pockets
{
    [CreateAssetMenu(fileName = "Ball Potted Channel" , menuName = "Balls/Events/Potted Channel" , order = 0)]
    public class BallPocketedChannel : AbstractEventChannel<BallPocketedEvent>
    {
    }

    public struct BallPocketedEvent
    {
        public BallController PottedBall;
        public PocketController Pocket;
    }
    
}
