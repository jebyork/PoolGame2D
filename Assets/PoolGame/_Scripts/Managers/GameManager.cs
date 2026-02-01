using System;
using PoolGame.Events;
using UnityEngine;

namespace PoolGame.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameStateChangeChannel gameStateChangeChannel;
        [SerializeField] private ShotRequestedChannel shotRequestedChannel;
        [SerializeField] private BoolEventChannel ballsStoppedMovingEventChannel;
        private GameState _gameState;

        private void OnEnable()
        {
            shotRequestedChannel?.Subscribe(ShotRequested);
            ballsStoppedMovingEventChannel?.Subscribe(BallsStoppedMoving);
        }
        
        private void OnDisable()
        {
            shotRequestedChannel?.Unsubscribe(ShotRequested);
            ballsStoppedMovingEventChannel?.Unsubscribe(BallsStoppedMoving);
        }
        
        private void BallsStoppedMoving(bool data)
        {
            if (data)
            {
                ChangeGameState(GameState.PlayerTurn);
            }
        }
        
        private void ShotRequested(ShotData data)
        {
            if (data.ShotPower01 > 0)
            {
                ChangeGameState(GameState.BallsInPlay);
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
            gameStateChangeChannel.RaiseEvent(new GameStateChange(previousState, _gameState));
        }
    }

    public enum GameState
    {
        Setup,
        PlayerTurn,
        BallsInPlay
    }
}
