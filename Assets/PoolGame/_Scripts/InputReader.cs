using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PoolGame
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "InputReader")]
    public class InputReader : ScriptableObject , GameInputActions.IGameplayActions
    {
        private GameInputActions _gameInputActions;

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
        
        public event Action OnPressEvent;
        public event Action OnPressCanceledEvent;
        
        public event Action<Vector2> OnCursorEvent;
        
        public void OnMove(InputAction.CallbackContext context)
        {
            OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
        }
        
        public void OnPress(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    OnPressEvent?.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    OnPressCanceledEvent?.Invoke();
                    break;
            }
        }

        public void OnCursor(InputAction.CallbackContext context)
        {
            OnCursorEvent?.Invoke(context.ReadValue<Vector2>());
        }
        

    }
}
