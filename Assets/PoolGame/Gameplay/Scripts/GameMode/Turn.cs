using PoolGame.Game.Attribute;

namespace PoolGame.Gameplay.GameMode
{
    public class Turn : Attribute
    {
        public override void DecreaseAttribute(int amount)
        {
            AttributeValue += amount;
        }

        public override void IncreaseAttribute(int amount)
        {
            AttributeValue -= amount;
        }

        public override void ResetAttribute()
        {
            AttributeValue = 0;
        }
    }
}