namespace PoolGame.Gameplay.Ball.Spawning
{
    public class SingleBallSpawner : BallSpawner
    {
        protected override void Spawn()
        {
            SpawnBall(transform.position);
        }
    }
}