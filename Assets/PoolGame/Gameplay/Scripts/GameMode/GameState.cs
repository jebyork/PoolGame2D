using System;

using UnityEngine;

namespace PoolGame.Gameplay.GameMode
{
    public enum GameStateEnum
    {
        Entry,
        Starting,
        AwaitingTurn,
        TurnInProgress,
        TurnEvaluation,
        Finished
    }

    [RequireComponent(typeof(Turn))]
    public class GameState : MonoBehaviour
    {
        public GameStateEnum Current { get; private set; }
        
        private Turn _turn;

        private void Awake()
        {
            _turn = GetComponent<Turn>();
        }

        public event Action<GameStateEnum> Changed;

        public void Set(GameStateEnum state)
        {
            if (Current == state) return;

            Current = state;
            Changed?.Invoke(Current);
        }
        
        public void AdvanceTurn()
        {
            int turnIncrease = 1;
            _turn.IncreaseAttribute(turnIncrease);
        }
        
        public int GetTurn() => _turn.GetAttributeValue();
    }
}
