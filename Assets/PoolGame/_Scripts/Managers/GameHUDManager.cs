using System;
using PoolGame.Events;
using UnityEngine;
using UnityEngine.UIElements;

namespace PoolGame.Managers
{
    public class GameHUDManager : MonoBehaviour
    {
        [SerializeField] private GameStateChangeChannel gameStateChangeChannel;
        
        private Label _gameStateLabel;

        private void OnEnable()
        {
            gameStateChangeChannel?.Subscribe(GameStateChanged);
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            _gameStateLabel = root.Q<Label>("StateLabel");

        }
        
        private void OnDisable()
        {
            gameStateChangeChannel?.Unsubscribe(GameStateChanged);
        }
        
        private void GameStateChanged(GameStateChange data)
        {
            _gameStateLabel.text = data.To.ToString();
        }
    }
}
