using PoolGame.Core.Events.Channels;
using UnityEngine;

namespace PoolGame.Core.Game.States.Gameplay
{
    [CreateAssetMenu(fileName = "Game State Change Chanel", menuName = "Events/Game State Change")]
    public class GameplayStateChangeChannel : AbstractEventChannel<GameStateChange> {}
    
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
