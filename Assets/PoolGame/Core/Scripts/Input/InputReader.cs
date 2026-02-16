using System;
using PoolGame.Core.Observers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PoolGame.Core.Input
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "InputReader")]
    public class InputReader : ScriptableObject , GameInputActions.IGameplayActions
    {
        private GameInputActions _gameInputActions;
        [SerializeField] private ObservableVector2 mouseScreenPosition;

        
        private void OnEnable()
        {
            if (_gameInputActions == null)
            {
                _gameInputActions = new GameInputActions();
                _gameInputActions.Gameplay.SetCallbacks(this);
            }

            EnableGameplay();
        }

        private void OnDisable()
        {
            _gameInputActions?.Gameplay.Disable();
        }
        
        private void EnableGameplay()
        {
            _gameInputActions?.Gameplay.Enable();
        }
        
        public event Action<Vector2> OnMoveEvent;
        public event Action OnPressedEvent;
        public event Action OnReleasedEvent;
        
        
        
        public void OnMove(InputAction.CallbackContext context)
        {
            Debug.Log("Move");
            OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
        }
        
        public void OnPress(InputAction.CallbackContext context)
        {
            Debug.Log("Press");
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    OnPressedEvent?.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    OnReleasedEvent?.Invoke();
                    break;
            }
        }

        public void OnCursor(InputAction.CallbackContext context)
        {
            Debug.Log("Cursor");
            mouseScreenPosition.Value = context.ReadValue<Vector2>();
        }
    }
}
