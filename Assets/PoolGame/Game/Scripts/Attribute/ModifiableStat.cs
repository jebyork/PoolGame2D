using System.Collections.Generic;
using UnityEngine;

namespace PoolGame.Game.Attribute
{
    [System.Serializable]
    public class ModifiableStat
    {
        [SerializeField] private float baseValue;

        private readonly List<StatModifier> _modifiers = new();

        public float Value => CalculateFinalValue();

        private float CalculateFinalValue()
        {
            float additive = 0f;
            float multiplier = 1f;

            foreach (StatModifier mod in _modifiers)
            {
                if (mod.type == ModifierType.Additive)
                    additive += mod.value;
                else
                    multiplier *= mod.value;
            }

            return (baseValue + additive) * multiplier;
        }

        public void AddModifier(StatModifier modifier) => _modifiers.Add(modifier);

        public void RemoveFromSource(object source) =>
            _modifiers.RemoveAll(m => m.Source == source);

        public void Update()
        {
            for (int i = _modifiers.Count - 1; i >= 0; i--)
            {
                if (_modifiers[i].remainingTurns == -1)
                    continue;

                _modifiers[i].remainingTurns--;

                if (_modifiers[i].remainingTurns <= 0)
                    _modifiers.RemoveAt(i);
            }
        }
    }
}
