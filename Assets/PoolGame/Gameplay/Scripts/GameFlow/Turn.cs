using PoolGame.Game.Attribute;
namespace PoolGame.Gameplay.GameFlow
{
    public class Turn : Attribute
    {
	    void Update()
	    {
		    Logwin.Log("Turn", AttributeValue, "Turn");
	    }
	    
        public override void DecreaseAttribute(int amount)
        {
            AttributeValue -= amount;
        }

        public override void IncreaseAttribute(int amount)
        {
            AttributeValue += amount;
        }

        public override void ResetAttribute()
        {
            AttributeValue = 0;
        }
    }
}