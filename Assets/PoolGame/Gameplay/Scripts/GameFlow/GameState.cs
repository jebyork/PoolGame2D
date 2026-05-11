using System;
using UnityEngine;
namespace PoolGame.Gameplay.GameFlow
{
    public enum EGameState
    {
        Entry,
        Setup,
        InPlay,
        Finished
    }

    [RequireComponent(typeof(Turn))]
    public class GameState : MonoBehaviour
    {
        public EGameState Current { get; private set; }
        
        public event Action<EGameState> Changed;

        public void Set(EGameState state)
        {
            if (Current == state) return;

            Current = state;
            Changed?.Invoke(Current);
        }
    }
}
