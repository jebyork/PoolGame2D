namespace PoolGame.Game.Attribute
{
    [System.Serializable]
    public enum ModifierType { Additive, Multiplicative }

    [System.Serializable]
    public class StatModifier
    {
        public float value;
        public ModifierType type;
        public int remainingTurns; 
        public object Source;
    }
}
