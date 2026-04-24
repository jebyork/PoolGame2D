using System;

namespace PoolGame.Gameplay.GameMode
{
    public interface ITurnOutcomeHandler
    {
        void OnTurnEvaluate(Action onComplete);
    }
}
