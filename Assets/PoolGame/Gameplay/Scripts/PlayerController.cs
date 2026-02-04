using PoolGame.Core.Game.States.Gameplay;
using PoolGame.Core.Input;
using PoolGame.Gameplay.Aim;
using PoolGame.Gameplay.Shot;
using PoolGame.Gameplay.ShotTargetPicker;
using UnityEngine;

namespace PoolGame.Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private InputReader inputReader;
        [SerializeField] private AimUpdatedChannel aimUpdatedChannel;
        
        [Space,  Header("Strategy's")]
        [SerializeField] private ShotTargetPickerStrategy shotTargetPickerStrategy;
        [SerializeField] private AimStrategyFactory aimStrategyFactory;
        [SerializeField] private ShotValidatorStrategy shotValidator;
        [SerializeField] private ShotCommandStrategy shotCommand;
        
        private bool _isPulling;
        private Vector3 _cursorClickPosition;
        private AimSnapshot _latestAimSnapshot;
        private bool _hadValidSnapshotThisPull;
        private PlayerShotInteractor _interactor;
        private IShootable _currentShootable;
  
        

        private bool _canGo;
        
        
        private void OnEnable()
        {
            if (inputReader == null) return;
            inputReader.OnPressEvent += OnPressedStarted;
            inputReader.OnPressCanceledEvent += OnPressedCanceled;
        }
        
        private void OnDisable()
        {
            if (inputReader == null) return;
            inputReader.OnPressEvent -= OnPressedStarted;
            inputReader.OnPressCanceledEvent -= OnPressedCanceled;
        }
        
        private void Awake()
        {
            IAimStrategy aim = aimStrategyFactory != null ? aimStrategyFactory.CreateAimStrategy() : new NullAimStrategy();
            aim ??= new NullAimStrategy();

            _interactor = new PlayerShotInteractor(
                shotTargetPickerStrategy,
                aim,
                shotValidator,
                shotCommand,
                aimUpdatedChannel);
        }

        private void Update() => _interactor.Tick();

        private void OnPressedStarted() => _interactor.PressStarted();
        private void OnPressedCanceled() => _interactor.PressCanceled();

        public void OnGameStateChange(GameStateChange data)
            => _interactor.SetCanGo(data.To == GameState.PlayerTurn);
    }
}
