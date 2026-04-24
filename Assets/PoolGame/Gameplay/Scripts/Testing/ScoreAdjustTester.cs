using PoolGame.Gameplay.Attributes;
using UnityEngine;

namespace PoolGame.Gameplay.Testing
{
    public class ScoreAdjustTester : MonoBehaviour
    {
        [SerializeField] private Score scoreAttribute;

        // Current Score: F/G to decrease/increase
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F)) scoreAttribute.DecreaseAttribute(Random.Range(0, 999));
            if (Input.GetKeyDown(KeyCode.G)) scoreAttribute.IncreaseAttribute(Random.Range(0, 999));
        }
    }
}