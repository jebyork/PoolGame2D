using System;
using PoolGame.Core.Events.Channels;
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
        [SerializeField] private NoDataEventChannel startedAimingEvent;
        [SerializeField] private NoDataEventChannel stoppedAimingEvent;
        
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
        
        public void OnMove(InputAction.CallbackContext context)
        {
            OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
        }
        
        public void OnPress(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    startedAimingEvent?.RaiseEvent(Unit.Default);
                    break;
                case InputActionPhase.Canceled:
                    stoppedAimingEvent?.RaiseEvent(Unit.Default);
                    break;
            }
        }

        public void OnCursor(InputAction.CallbackContext context)
        {
            mouseScreenPosition.Value = context.ReadValue<Vector2>();
        }
        

    }
}
