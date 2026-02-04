using PoolGame.Gameplay.Aim;
using UnityEngine;

namespace PoolGame.Gameplay.Shot
{
    public abstract class ShotValidatorStrategy : ScriptableObject, IShotValidator
    {
        public abstract bool IsValid(ShotCommandContext context);
    }
}
