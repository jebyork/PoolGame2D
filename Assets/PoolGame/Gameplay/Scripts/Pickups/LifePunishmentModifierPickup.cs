using PoolGame.Game.Attribute;
using PoolGame.Gameplay.GameMode;
using UnityEngine;

namespace PoolGame.Gameplay.Pickups
{
    [CreateAssetMenu(fileName = "Life Punishment Pickup Effect", menuName = "Pickup/Life Punishment", order = 0)]
    public class LifePunishmentModifierPickup : PickupEffect
    {
        [SerializeField] private int lifePunishment = 1;
        
        [SerializeField] private int remainingTurns = 3;
        
        public override void PlayEffect(GameObject pickup)
        {
            TurnModifiers turnModifiers = FindFirstObjectByType<TurnModifiers>();
            turnModifiers.lifePunishment.AddModifier(new StatModifier
            {
                value = lifePunishment,
                type = ModifierType.Additive,
                remainingTurns = remainingTurns,
                Source = this
            });
        }
    }
}