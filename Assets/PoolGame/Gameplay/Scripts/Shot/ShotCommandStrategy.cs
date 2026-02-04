using PoolGame.Gameplay.Aim;
using UnityEngine;

namespace PoolGame.Gameplay.Shot
{
    public abstract class ShotCommandStrategy : ScriptableObject, IShotCommand
    {
        public abstract void Execute(ShotCommandContext context);
    }
    
    public struct ShotCommandContext
    {
        public IShootable Shootable;
        public AimSnapshot Snapshot;
    }
}
