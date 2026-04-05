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

        protected void SpawnBall(Vector3 position)
        {
            GameObject ballObject = Instantiate(
                ballPrefab,
                position,
                Quaternion.identity,
                transform
            );

            BallController ballController = ballObject.GetComponent<BallController>();
            if (ballController != null) return;
            Debug.LogError($"[Ball Spawner] Ball Controller is missing on {ballObject.name}");
            Destroy(ballObject);

        }
    }
}