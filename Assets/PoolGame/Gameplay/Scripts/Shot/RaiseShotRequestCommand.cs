using PoolGame.Core.Game.States.Gameplay.Shot;
using UnityEngine;

namespace PoolGame.Gameplay.Shot
{
    [CreateAssetMenu(fileName = "Raise Shot Requested Command", menuName = "Shot Command/Raise Event")]
    public class RaiseShotRequestedCommand : ShotCommandStrategy
    {
        [SerializeField] private ShotRequestedChannel shotRequestedChannel;

        public override void Execute(ShotCommandContext context)
        {
            if (shotRequestedChannel == null || context.Shootable == null)
                return;

            ShotData shotData = new()
            {
                Shootable = context.Shootable,
                ShotDirection = context.Snapshot.ShotDirection,
                ShotPower01 = context.Snapshot.ShotPower01
            };

            shotRequestedChannel.RaiseEvent(shotData);
        }
    }
}
