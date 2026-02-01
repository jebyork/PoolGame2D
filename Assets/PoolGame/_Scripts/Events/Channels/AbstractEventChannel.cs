using UnityEngine;
using UnityEngine.Events;

namespace PoolGame.Events
{
    public abstract class AbstractEventChannel<T> : ScriptableObject
    {
        private UnityAction<T> _onEventRaised;
        
        public void RaiseEvent(T parameter)
        {
            _onEventRaised?.Invoke(parameter);
        }

        public void Subscribe(UnityAction<T> handler)
        {
            _onEventRaised += handler;
        }

        public void Unsubscribe(UnityAction<T> handler)
        {
            _onEventRaised -= handler;
        } 
    }
}