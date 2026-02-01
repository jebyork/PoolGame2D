using UnityEngine;
using UnityEngine.Events;

namespace PoolGame.Events
{
    public abstract class AbstractEventListener<TEventChannel, TEventType> : MonoBehaviour where TEventChannel : AbstractEventChannel<TEventType>
    {
        [Header("Listen to Event")]
        [SerializeField] private TEventChannel eventChannel;
        [Tooltip("Response to Received Event")]
        [SerializeField] private UnityEvent<TEventType> response;
        
        protected virtual void OnEnable()
        {
            if (!eventChannel)
                return;
            eventChannel.Subscribe(OnEventRaised);
        }
        
        protected virtual void OnDisable()
        {
            if (!eventChannel)
                return;
            eventChannel.Unsubscribe(OnEventRaised);
        }
        
        private void OnEventRaised(TEventType args)
        {
            response?.Invoke(args);
        }
    }
}