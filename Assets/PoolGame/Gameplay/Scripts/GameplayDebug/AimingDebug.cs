using PoolGame.Core.JebDebug;
using PoolGame.Gameplay;
using PoolGame.Gameplay.Aim;
using UnityEngine;

namespace PoolGame.Core.Game.States.Gameplay.GameplayDebug
{
    public class AimingDebug : AbstractDebug
    {
        [SerializeField] private AimUpdatedChannel aimUpdatedChannel;
        private AimSnapshot _lastAimSnapshot;

        private void OnEnable()
        {
            if (aimUpdatedChannel)
            {
                aimUpdatedChannel.Subscribe(OnAiming);
            }
        }
        
        private void OnDisable()
        {
            if (aimUpdatedChannel)
            {
                aimUpdatedChannel.Unsubscribe(OnAiming);
            }
        }
        
        private void OnAiming(AimSnapshot data)
        {
            _lastAimSnapshot = data;
        }
        
        protected override void LogDebug()
        {
            Logwin.Log("Aiming Point: ", _lastAimSnapshot.AimingPoint, "Aiming Data");
            Logwin.Log("Cursor World Point: ", _lastAimSnapshot.CursorWorldPosition, "Aiming Data");
            Logwin.Log("Clamped End Aiming Point: ", _lastAimSnapshot.AimEndPoint, "Aiming Data");
            Logwin.Log("Shot Direction: ", _lastAimSnapshot.ShotDirection, "Aiming Data");
            Logwin.Log("Clamped Pull Distance: ", _lastAimSnapshot.PullDistance, "Aiming Data");
            Logwin.Log("Clamped Pull Percentage: ", _lastAimSnapshot.ShotPower01, "Aiming Data");
        }
    }
}
