using PoolGame.Core.Events.Channels;
using UnityEngine;

namespace PoolGame.Gameplay.Aim
{
    [CreateAssetMenu(fileName = "Aim Update Chanel", menuName = "Events/Aim Update Chanel")]
    public class AimUpdatedChannel : AbstractEventChannel<AimSnapshot> {}
}
