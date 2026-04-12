using PoolGame.Core.Events;
using UnityEngine;
using UnityEngine.Serialization;

namespace PoolGame.Gameplay.Ball.Spawning
{
    public abstract class BallSpawner : MonoBehaviour
    {
        [FormerlySerializedAs("onSpawnBalls")]
        [SerializeField] protected VoidEventChannel spawnCommandChannel;
        [SerializeField] protected GameObject ballPrefab;
        
        [SerializeField] protected BallContainer ballContainer;

        private void OnEnable()
        {
            spawnCommandChannel.Subscribe(Spawn);
        }

        private void OnDisable()
        {
            spawnCommandChannel.Unsubscribe(Spawn);
        }

        public abstract void Spawn();

        protected BallController SpawnBall(Vector3 position)
        {
            return ballContainer.SpawnBall(ballPrefab, position, transform);
        }
    }
}
