using UnityEngine;

namespace PoolGame.Line
{
    public abstract class AbstractLine : MonoBehaviour
    {
        [SerializeField] protected LineRenderer LineR;
        
        public void SetActive(bool active)
        {
            if (LineR != null)
            {
                LineR.enabled = active;
            }
        }
    }
}
