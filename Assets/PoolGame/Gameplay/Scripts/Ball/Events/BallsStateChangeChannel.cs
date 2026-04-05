using UnityEngine;
using UnityEngine.Events;

namespace PoolGame.Gameplay.Ball.Events
{
    [CreateAssetMenu(fileName = "Ball State Change", menuName = "Events/Ball/State Change", order = 0)]
    public class BallsStateChangeChannel : ScriptableObject
    {
        private UnityAction<BallState> _onEventRaised;

        public void RaiseEvent(BallState state)
        {
            _onEventRaised?.Invoke(state);
        }

        public void Subscribe(UnityAction<BallState> action)
        {
            _onEventRaised += action;
        }

        public void Unsubscribe(UnityAction<BallState> action)
        {
            _onEventRaised -= action;
        }
    }
    
    [System.Serializable]
    public enum BallState{
        Moving,
        Stopped
    }
}