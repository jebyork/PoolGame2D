using UnityEngine;

namespace PoolGame.Core.Game.Line
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
