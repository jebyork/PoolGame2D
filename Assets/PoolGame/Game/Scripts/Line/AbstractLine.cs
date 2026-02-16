using System;
using UnityEngine;

namespace PoolGame.Game.Line
{
    public abstract class AbstractLine : MonoBehaviour
    {
        [SerializeField] protected LineRenderer lineR;

        protected virtual void Awake()
        {
            if (lineR) return;
            
            lineR = GetComponent<LineRenderer>();
            if (lineR) return;
            
            Debug.LogError($"LineRenderer not found on {gameObject.name}");
        }

        public void SetActive(bool active)
        {
            if (lineR != null)
            {
                lineR.enabled = active;
            }
        }
    }
}
