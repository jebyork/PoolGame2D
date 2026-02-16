using UnityEngine;
using UnityEngine.Events;

namespace PoolGame.Core.Input
{
    public class InputReaderListener : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        
        public UnityEvent<Vector2> OnMove;
        public UnityEvent OnPressed;
        public UnityEvent OnReleased;

        private void OnEnable()
        {
            if (inputReader == null) return;

            inputReader.OnMoveEvent += HandleMove;
            inputReader.OnPressedEvent += HandlePressed;
            inputReader.OnReleasedEvent += HandleReleased;
        }

        private void OnDisable()
        {
            if (inputReader == null) return;

            inputReader.OnMoveEvent -= HandleMove;
            inputReader.OnPressedEvent -= HandlePressed;
            inputReader.OnReleasedEvent -= HandleReleased;
        }

        private void HandleMove(Vector2 value)
        {
            OnMove?.Invoke(value);
        }

        private void HandlePressed()
        {
            OnPressed?.Invoke();
        }

        private void HandleReleased()
        {
            OnReleased?.Invoke();
        }
    }
}