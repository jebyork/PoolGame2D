using UnityEngine;

namespace PoolGame.Core.Values
{
    public abstract class AbstractValue<T> : ScriptableObject
    {
        [SerializeField] private T value;

        public virtual T Value
        {
            get => value;
            set => this.value = value;
        }
    }
}