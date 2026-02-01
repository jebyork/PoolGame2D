using UnityEngine;

namespace PoolGame.JebDebug
{
    public abstract class AbstractDebug : MonoBehaviour
    {
        [SerializeField] private bool showDebug;

        private void Update()
        {
            if (showDebug)
            {
                LogDebug();
            }
        }
        protected abstract void LogDebug();
    }
}
