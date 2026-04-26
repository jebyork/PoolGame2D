using PoolGame.Game.Attribute;
using PoolGame.Gameplay.GameMode;
using UnityEngine;

namespace PoolGame.Gameplay.Pickups
{
    [CreateAssetMenu(fileName = "Score Multiplier Pickup Effect", menuName = "Pickup/Score Multiplier", order = 0)]
    public class ScoreMultiplierModifierPickup : PickupEffect
    {
        [SerializeField] private int scoreMultiplier = 2;
        
        [SerializeField] private int remainingTurns = 3;
        
        public override void PlayEffect(GameObject pickup)
        {
            TurnModifiers turnModifiers = FindFirstObjectByType<TurnModifiers>();
            turnModifiers.scorePerObjectBall.AddModifier(new StatModifier
            {
                value = scoreMultiplier,
                type = ModifierType.Multiplicative,
                remainingTurns = remainingTurns,
                Source = this
            });
        }
    }
}