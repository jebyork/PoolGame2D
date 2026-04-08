using PoolGame.Core.Events;
using UnityEngine;

namespace PoolGame.Gameplay.Ball.Spawning
{
    public abstract class BallSpawner : MonoBehaviour
    {
        [SerializeField] protected VoidEventChannel onSpawnBalls;
        [SerializeField] protected GameObject ballPrefab;

        private void OnEnable()
        {
            onSpawnBalls.Subscribe(Spawn);
        }

        private void OnDisable()
        {
            onSpawnBalls.Unsubscribe(Spawn);
        }

        protected abstract void Spawn();

        protected BallController SpawnBall(Vector3 position)
        {
            return BallContainer.Instance.SpawnBall(ballPrefab, position, transform);
        }
    }
}
