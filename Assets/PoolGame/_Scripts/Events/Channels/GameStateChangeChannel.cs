using PoolGame.Managers;
using UnityEngine;

namespace PoolGame.Events
{
    [CreateAssetMenu(fileName = "Game State Change Chanel", menuName = "Events/Game State Change")]
    public class GameStateChangeChannel : AbstractEventChannel<GameStateChange> {}
    
    public readonly struct GameStateChange
    {
        public readonly GameState From;
        public readonly GameState To;

        public GameStateChange(GameState from, GameState to)
        {
            From = from;
            To = to;
        }
    }
}
