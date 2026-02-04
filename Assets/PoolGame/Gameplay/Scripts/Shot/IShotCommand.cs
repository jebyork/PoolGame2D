namespace PoolGame.Gameplay.Shot
{
    public interface IShotCommand
    {
        void Execute(ShotCommandContext context);
    }
}
