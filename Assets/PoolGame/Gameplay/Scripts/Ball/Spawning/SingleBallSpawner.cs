namespace PoolGame.Gameplay.Ball.Spawning
{
    public class SingleBallSpawner : BallSpawner
    {
        public override void Spawn()
        {
            SpawnBall(transform.position);
        }
    }
}