using PoolGame.Gameplay.Aim;

namespace PoolGame.Gameplay.Shot
{
    public interface IShotValidator
    {
        bool IsValid(ShotCommandContext context);
    }
}
