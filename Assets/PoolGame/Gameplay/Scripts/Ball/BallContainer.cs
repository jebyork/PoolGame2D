using System.Collections.Generic;
using System.Linq;
using PoolGame.Core;
using UnityEngine;

namespace PoolGame.Gameplay.Ball
{
    public class BallContainer : MonoBehaviour
    {
        [Space]
        [SerializeField] private List<BallController> pooledBalls = new();
        [SerializeField] private List<BallController> activeBalls = new();

        public IReadOnlyList<BallController> ActiveBalls => activeBalls;

        private readonly Dictionary<GameObject, Queue<BallController>> _poolsByPrefab = new();
        private readonly Dictionary<BallController, GameObject> _prefabByBall = new();

        public BallController SpawnBall(GameObject prefab, Vector3 position, Transform parent)
        {
            if (prefab == null)
            {
                Debug.LogError("[Ball Container] Can't spawn a null prefab.");
                return null;
            }

            BallController ball = GetAvailableBall(prefab);
            if (ball == null)
                return null;

            pooledBalls.Remove(ball);

            if (!activeBalls.Contains(ball))
            {
                activeBalls.Add(ball);
            }

            ball.Activate(position, parent);
            return ball;
        }

        public void ReleaseAll()
        {
            foreach (BallController ball in ActiveBalls.Reverse())
            {
                ReleaseBall(ball);
            }
        }

        public void ReleaseBall(BallController ball)
        {
            if (ball == null)
                return;

            if (!_prefabByBall.TryGetValue(ball, out GameObject prefab))
            {
                Debug.LogWarning($"[Ball Container] Cannot release untracked ball {ball.name}.", ball);
                return;
            }

            activeBalls.Remove(ball);

            if (!pooledBalls.Contains(ball))
            {
                pooledBalls.Add(ball);
            }

            Queue<BallController> pool = GetOrCreatePool(prefab);
            if (!pool.Contains(ball))
            {
                pool.Enqueue(ball);
            }

            ball.Deactivate();
        }

        public BallController GetPooledBallOfType(BallType ballType)
        {
            foreach (BallController ball in pooledBalls)
            {
                if (ball != null && ball.BallType == ballType)
                {
                    return ball;
                }
            }

            return null;
        }

        public int GetActiveBallCount(BallType ballType)
        {
            int count = 0;

            foreach (BallController ball in activeBalls)
            {
                if (ball != null && ball.BallType == ballType)
                {
                    count++;
                }
            }

            return count;
        }

        private BallController GetAvailableBall(GameObject prefab)
        {
            Queue<BallController> pool = GetOrCreatePool(prefab);

            while (pool.Count > 0)
            {
                BallController pooledBall = pool.Dequeue();
                if (pooledBall != null)
                {
                    return pooledBall;
                }
            }

            GameObject ballObject = Instantiate(prefab);
            BallController ball = ballObject.GetComponent<BallController>();
            if (ball == null)
            {
                Debug.LogError($"[Ball Container] Ball Controller is missing on {ballObject.name}");
                Destroy(ballObject);
                return null;
            }

            _prefabByBall[ball] = prefab;
            return ball;
        }

        private Queue<BallController> GetOrCreatePool(GameObject prefab)
        {
            if (_poolsByPrefab.TryGetValue(prefab, out Queue<BallController> pool))
            {
                return pool;
            }

            pool = new Queue<BallController>();
            _poolsByPrefab[prefab] = pool;
            return pool;
        }
    }
}
