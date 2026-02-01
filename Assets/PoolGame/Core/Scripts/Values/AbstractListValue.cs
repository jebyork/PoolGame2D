using System.Collections.Generic;
using UnityEngine;

namespace PoolGame.Core.Values
{
    public class AbstractListValue<T> : AbstractValue<List<T>>
    {
        public T Get(int index)
        {
            if (Value == null || index < 0 || index >= Value.Count)
            {
                Debug.LogWarning($"[AbstractListValue] Invalid index {index} on {name}");
                return default;
            }

            return Value[index];
        }
        
        public void Set(int index, T item)
        {
            if (Value == null || index < 0 || index >= Value.Count)
            {
                Debug.LogWarning($"[AbstractListValue<{typeof(T).Name}>] Invalid Set at index {index} in {name}");
                return;
            }

            Value[index] = item;
        }
        
        public void Add(T item)
        {
            if (Value == null)
                Value = new List<T>();

            Value.Add(item);
        }

        public void RemoveAt(int index)
        {
            if (Value == null || index < 0 || index >= Value.Count)
            {
                Debug.LogWarning($"[AbstractListValue<{typeof(T).Name}>] Invalid RemoveAt index {index} in {name}");
                return;
            }

            Value.RemoveAt(index);
        }
        
        public bool Remove(T item)
        {
            if (Value == null)
                return false;

            return Value.Remove(item);
        }
        
        public void Clear()
        {
            if (Value == null)
                return;

            Value.Clear();
        }

        public T GetRandom()
        {
            if (Value == null || Value.Count == 0)
            {
                Debug.LogWarning($"[AbstractListValue] No  List on {name}");
                return default;
            }
            
            int randomIndex = Random.Range(0, Value.Count);
            return Get(randomIndex);
        }
    }
}