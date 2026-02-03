using PoolGame.Core.Game.States.Gameplay;
using PoolGame.Core.Game.States.Gameplay.Shot;
using PoolGame.Gameplay.Shot;
using UnityEngine;

namespace PoolGame.Gameplay.ShotTargetPicker
{
    public abstract class ShotTargetPickerStrategy : ScriptableObject , IShotTargetPicker
    {
        public abstract ShotTargetPickResult TryPick();
    }
}