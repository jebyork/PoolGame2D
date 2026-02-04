using PoolGame.Gameplay.Aim;
using PoolGame.Gameplay.Shot;
using PoolGame.Gameplay.ShotTargetPicker;
using UnityEngine;

namespace PoolGame.Gameplay
{
    public sealed class PlayerShotInteractor
    {
        private readonly IShotTargetPicker _picker;
        private readonly IAimStrategy _aim;
        private readonly IShotValidator _validator;
        private readonly IShotCommand _command;
        private readonly AimUpdatedChannel _aimUpdated;

        private bool _canGo;
        private bool _isPulling;

        private bool _hadValidSnapshotThisPull;
        private AimSnapshot _latestSnapshot;

        private IShootable _shootable;

        public PlayerShotInteractor(
            IShotTargetPicker picker,
            IAimStrategy aim,
            IShotValidator validator,
            IShotCommand command,
            AimUpdatedChannel aimUpdatedChannel)
        {
            _picker = picker;
            _aim = aim;
            _validator = validator;
            _command = command;
            _aimUpdated = aimUpdatedChannel;
        }

        public void SetCanGo(bool canGo)
        {
            _canGo = canGo;

            if (!_canGo)
                CancelPull(); // ends aim + clears state
        }

        public void PressStarted()
        {
            if (!_canGo) return;
            if (_isPulling) return; // ignore re-entrant starts
            if (_picker == null) return;

            ShotTargetPickResult target = _picker.TryPick();
            if (!target.HasHit)
                return;

            _shootable = target.Target;

            Vector3 start = target.HitPoint;
            start.z = 0f;

            _aim.Begin(start);

            _isPulling = true;
            _hadValidSnapshotThisPull = false;
        }

        public void Tick()
        {
            if (!_canGo) return;
            if (!_isPulling) return;

            if (_aim.TryGetSnapshot(out AimSnapshot snapshot))
            {
                _latestSnapshot = snapshot;
                _hadValidSnapshotThisPull = true;
                _aimUpdated?.RaiseEvent(snapshot);
            }
        }

        public void PressCanceled()
        {
            if (!_isPulling) return;
            
            _aim.End();
            _isPulling = false;
            
            if (!_canGo || !_hadValidSnapshotThisPull || _command == null || _shootable == null)
            {
                ResetShotState();
                return;
            }

            ShotCommandContext context = new ShotCommandContext
            {
                Shootable = _shootable,
                Snapshot = _latestSnapshot
            };

            bool isValid = _validator == null || _validator.IsValid(context);
            if (isValid)
                _command.Execute(context);

            ResetShotState();
        }

        private void CancelPull()
        {
            if (_isPulling)
            {
                _aim.End();
                _isPulling = false;
            }

            ResetShotState();
        }

        private void ResetShotState()
        {
            _hadValidSnapshotThisPull = false;
            _shootable = null;
        }
    }
}
