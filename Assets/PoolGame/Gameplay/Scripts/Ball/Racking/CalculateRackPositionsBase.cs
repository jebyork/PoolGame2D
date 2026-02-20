using UnityEngine;

namespace PoolGame.Gameplay.Ball.Racking
{
    public abstract class CalculateRackPositionsBase : ScriptableObject
    {
        public abstract Vector2[] Calculate(int ballCount, float ballRadius);
    }
}
