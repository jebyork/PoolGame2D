using UnityEngine;
using UnityEngine.Events;

namespace PoolGame.Core.Observers
{
    public abstract class Observer<T> : ScriptableObject
    {
        [SerializeField] private T value;
        public UnityEvent<T> OnValueChanged;
        
        public T Value {
            get => value;
            set => Set(value);
        }
        
        public static implicit operator T(Observer<T> value) => value.Value;

        public void Set(T val)
        {
            if (Equals(this.value , val)) return;
            value = val;
            Invoke();
        }

        public void Invoke()
        {
            OnValueChanged?.Invoke(value);
        }

        public void AddListener(UnityAction<T> listener)
        {
            if (listener == null) return;
            if (OnValueChanged == null) OnValueChanged = new UnityEvent<T>();
            
            OnValueChanged.AddListener(listener);
        }

        public void RemoveListener(UnityAction<T> listener)
        {
            if (listener == null) return;
            if (OnValueChanged == null) return;
            
            OnValueChanged.RemoveListener(listener);
        }

        public void RemoveAllListeners()
        {
            if (OnValueChanged == null) return;
            
            OnValueChanged.RemoveAllListeners();
        }
    }
}
