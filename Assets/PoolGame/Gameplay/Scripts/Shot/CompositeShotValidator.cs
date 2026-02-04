using PoolGame.Gameplay.Aim;
using UnityEngine;

namespace PoolGame.Gameplay.Shot
{
    [CreateAssetMenu(fileName = "Composite Shot Validator", menuName = "Shot Validator/Composite")]
    public class CompositeShotValidator : ShotValidatorStrategy
    {
        [SerializeField] private ShotValidatorStrategy[] validators;

        public override bool IsValid(ShotCommandContext snapshot)
        {
            if (validators == null) return true;
            foreach (var v in validators)
                if (v != null && !v.IsValid(snapshot))
                    return false;
            return true;
        }
    }
}
