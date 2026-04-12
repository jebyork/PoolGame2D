using UnityEngine;
using UnityEngine.Events;

namespace PoolGame.Game
{
    public class CallFunctionOnStart : MonoBehaviour
    {
        [SerializeField] private bool callOnStart = true;
        public UnityEvent onStart;
        void Start()
        {
            if (callOnStart)
            {
                onStart?.Invoke();
            }
        }
    }
}
