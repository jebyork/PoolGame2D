using PoolGame.Gameplay.Ball;
using UnityEngine;
using UnityEngine.Events;

namespace PoolGame.Gameplay.Table.Pockets
{
    [CreateAssetMenu(fileName = "Ball Potted Channel" , menuName = "Events/Balls/Potted Channel" , order = 0)]
    public class BallPocketedChannel : ScriptableObject
    {
        private UnityAction<BallPocketedEvent> _onEventRaised;

        public void RaiseEvent(BallPocketedEvent pocketEvent)
        {
            _onEventRaised?.Invoke(pocketEvent);
        }

        public void Subscribe(UnityAction<BallPocketedEvent> action)
        {
            _onEventRaised += action;
        }

        public void Unsubscribe(UnityAction<BallPocketedEvent> action)
        {
            _onEventRaised -= action;
        }
        
    }

    public struct BallPocketedEvent
    {
        public BallController PottedBall;
        public PocketController Pocket;
    }
    
}
