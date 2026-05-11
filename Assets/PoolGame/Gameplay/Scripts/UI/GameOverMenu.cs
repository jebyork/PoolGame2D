using PoolGame.Gameplay.Attributes;
using PoolGame.Gameplay.GameFlow;
using UnityEngine;
using UnityEngine.UIElements;

namespace PoolGame.Gameplay.UI
{
    public class GameOverMenu : MonoBehaviour
    {
        [SerializeField] GameFlowController gameFlowController;
        [SerializeField] Score score;

        VisualElement _endScreen;
        Button _restartButton;
        Button _quitButton;
        Label _scoreLabel;
        

        void OnEnable()
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

        void OnDisable()
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

        void OnRestartClicked()
        {
            gameFlowController.ResetGame();
        }

        void OnQuitClicked()
        {
            Application.Quit();
        }
    }
}
