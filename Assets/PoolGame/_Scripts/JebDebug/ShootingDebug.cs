using PoolGame.Events;
using UnityEngine;

namespace PoolGame.JebDebug
{
    public class ShootingDebug : AbstractDebug
    {
        [SerializeField] private ShotRequestedChannel shotRequestedChannel;
        private ShotData _latestShotData;
        private void OnEnable()
        {
            if (shotRequestedChannel != null)
            {
                shotRequestedChannel.Subscribe(ShotRequested);
            }
        }

        private void OnDisable()
        {
            if (shotRequestedChannel != null)
            {
                shotRequestedChannel.Unsubscribe(ShotRequested);
            }
        }
        private void ShotRequested(ShotData data)
        {
            _latestShotData = data;
        }

        protected override void LogDebug()
        {
            Logwin.Log("Shot Target: ", _latestShotData.ShotTarget, "Shooting Data");
            Logwin.Log("Shot Direction: ", _latestShotData.ShotDirection, "Shooting Data");
            Logwin.Log("Shot Power: ", _latestShotData.ShotPower01, "Shooting Data");
        }
    }
}
