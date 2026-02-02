using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace PoolGame.Core.Game.States.Gameplay.UI
{
    public class GameplayHUDManager : MonoBehaviour
    {
        [FormerlySerializedAs("gameStateChangeChannel")] [SerializeField] private GameplayStateChangeChannel gameplayStateChangeChannel;
        
        private Label _gameStateLabel;

        private void OnEnable()
        {
            gameplayStateChangeChannel?.Subscribe(GameStateChanged);
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            _gameStateLabel = root.Q<Label>("StateLabel");

        }
        
        private void OnDisable()
        {
            gameplayStateChangeChannel?.Unsubscribe(GameStateChanged);
        }
        
        private void GameStateChanged(GameStateChange data)
        {
            _gameStateLabel.text = data.To.ToString();
        }
    }
}
