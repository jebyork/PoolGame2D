namespace PoolGame.Gameplay.Attributes
{
    public interface IAttribute
    {
        public void DecreaseAttribute(int amount);
        public void IncreaseAttribute(int amount);
        
        public int GetAttributeValue();
    }
}