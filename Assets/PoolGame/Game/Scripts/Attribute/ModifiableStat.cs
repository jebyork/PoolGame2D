using System.Collections.Generic;
using UnityEngine;

namespace PoolGame.Game.Attribute
{
    [System.Serializable]
    public class ModifiableStat
    {
        [SerializeField] private int baseValue;

        readonly List<StatModifier> _modifiers = new();

        public int Value => CalculateFinalValue();

        private int CalculateFinalValue()
        {
	        int additive = 0;
	        int multiplier = 1;

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
