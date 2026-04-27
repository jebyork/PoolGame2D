using System;
using PoolGame.Core.Helpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace PoolGame.Gameplay.Ball.Spawning
{
    public abstract class BallSpawner : MonoBehaviour
    {
        [FormerlySerializedAs("onSpawnBalls")]
        [SerializeField] protected GameObject ballPrefab;
        [SerializeField] protected BallContainer ballContainer;
        
        public abstract void Spawn();

        protected BallController SpawnBall(Vector3 position)
        {
            return ballContainer.SpawnBall(ballPrefab, position, transform);
        }
    }
}
