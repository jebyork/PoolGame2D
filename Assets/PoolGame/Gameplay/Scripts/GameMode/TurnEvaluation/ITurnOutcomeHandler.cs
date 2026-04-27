using System;

namespace PoolGame.Gameplay.GameMode.TurnEvaluation
{
    public interface ITurnOutcomeHandler
    {
        void OnTurnEvaluate(Action onComplete);
    }
}
