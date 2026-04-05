using UnityEngine;
using UnityEngine.Events;

namespace PoolGame.Core.Events
{
    [CreateAssetMenu(fileName = "Void Event", menuName = "Events/Core/Void", order = 0)]
    public class VoidEventChannel : ScriptableObject
    {
        private UnityAction _onEventRaised;

        public void RaiseEvent()
        {
            _onEventRaised?.Invoke();
        }

        public void Subscribe(UnityAction handler)
        {
            _onEventRaised += handler;
        }

        public void Unsubscribe(UnityAction handler)
        {
            _onEventRaised -= handler;
        } 
    }
}