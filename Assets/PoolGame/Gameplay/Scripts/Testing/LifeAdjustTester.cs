using PoolGame.Gameplay.Attributes;
using UnityEngine;

namespace PoolGame.Gameplay.Testing
{
    public class LifeAdjustTester : MonoBehaviour
    {
        [SerializeField] private Life lifeAttribute;

        // Current life: Q/E to decrease/increase
        // Max life:     Z/X to decrease/increase
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q)) lifeAttribute.DecreaseAttribute(1);
            if (Input.GetKeyDown(KeyCode.E)) lifeAttribute.IncreaseAttribute(1);
            if (Input.GetKeyDown(KeyCode.Z)) lifeAttribute.AdjustMaxLife(-1);
            if (Input.GetKeyDown(KeyCode.X)) lifeAttribute.AdjustMaxLife(1);
        }
    }
}