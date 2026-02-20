namespace PoolGame.Core.Setup
{
    public interface ISetupControl
    {
        public void Initialize();
        public void CreateObjects();
        public void Prepare();
        public void StartGame();
    }
}
