using System;
using PoolGame.Core.Events.Channels;
using PoolGame.Core.Game.States.Gameplay;
using UnityEngine;
using UnityEngine.Serialization;

namespace PoolGame.Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        [FormerlySerializedAs("gameStateChangeChannel")] [SerializeField] private GameplayStateChangeChannel gameplayStateChangeChannel;
        [SerializeField] private BoolEventChannel ballsStoppedMovingEventChannel;
        private GameState _gameState;

        private void OnEnable()
        {
            ballsStoppedMovingEventChannel?.Subscribe(BallsStoppedMoving);
        }
        
        private void OnDisable()
        {
            ballsStoppedMovingEventChannel?.Unsubscribe(BallsStoppedMoving);
        }
        
        private void BallsStoppedMoving(bool data)
        {
            if (data)
            {
                ChangeGameState(GameState.PlayerTurn);
            }
        }
        

        public void ChangeGameState(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Setup:
                    break;
                case GameState.PlayerTurn:
                    break;
                case GameState.BallsInPlay:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameState) , gameState , null);
            }
            
            GameState previousState = _gameState;
            _gameState = gameState;
            gameplayStateChangeChannel.RaiseEvent(new GameStateChange(previousState, _gameState));
        }
    }

    public enum GameState
    {
        Setup,
        PlayerTurn,
        BallsInPlay
    }
}
