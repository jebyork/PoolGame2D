using System;

namespace PoolGame.Gameplay.GameMode
{
    public interface IStartOutcomeHandler
    {
        void OnStartEffect(Action onComplete);
    }
}