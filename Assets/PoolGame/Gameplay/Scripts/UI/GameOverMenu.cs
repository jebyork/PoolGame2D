using System;
using PoolGame.Gameplay.Attributes;
using PoolGame.Gameplay.GameMode;
using UnityEngine;
using UnityEngine.UIElements;

namespace PoolGame.Gameplay.UI
{
    public class GameOverMenu : MonoBehaviour
    {
        [SerializeField] private GameController gameController;
        [SerializeField] private Score score;

        private VisualElement _endScreen;
        private Button _restartButton;
        private Button _quitButton;
        private Label _scoreLabel;
        

        private void OnEnable()
        {
            UIDocument document = GetComponent<UIDocument>();
            VisualElement root = document.rootVisualElement;

            _endScreen = root.Q<VisualElement>("EndScreen");
            _restartButton = _endScreen.Q<Button>("RestartButton");
            _quitButton = _endScreen.Q<Button>("QuitButton");
            _scoreLabel = _endScreen.Q<Label>("ScoreGameOverVal");

            _restartButton.clicked += OnRestartClicked;
            _quitButton.clicked += OnQuitClicked;
        }

        private void OnDisable()
        {
            if (_restartButton != null)
                _restartButton.clicked -= OnRestartClicked;

            if (_quitButton != null)
                _quitButton.clicked -= OnQuitClicked;
        }

        public void Show()
        {
            _endScreen.style.display = DisplayStyle.Flex;
            if(_scoreLabel != null)
                _scoreLabel.text = score.GetAttributeValue().ToString();
        }
        
        public void Hide()
        {
            _endScreen.style.display = DisplayStyle.None;
        }

        private void OnRestartClicked()
        {
            gameController.RestartGame();
        }

        private void OnQuitClicked()
        {
            Application.Quit();
        }
    }
}
