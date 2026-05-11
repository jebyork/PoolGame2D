using System.Collections.Generic;
using System.Linq;
using PoolGame.Core;
using UnityEngine;

namespace PoolGame.Gameplay.Ball
{
    public class BallContainer : MonoBehaviour
    {
	    readonly List<BallController> _pooledBalls = new();
        readonly List<BallController> _activeBalls = new();

        public IReadOnlyList<BallController> ActiveBalls => _activeBalls;

        readonly Dictionary<GameObject, Queue<BallController>> _poolsByPrefab = new();
        readonly Dictionary<BallController, GameObject> _prefabByBall = new();

        #region Public Functions For Ball (De)Spawning
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

            _pooledBalls.Remove(ball);

            if (!_activeBalls.Contains(ball))
            {
                _activeBalls.Add(ball);
            }

            ball.Activate(position, parent);
            return ball;
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

            _activeBalls.Remove(ball);

            if (!_pooledBalls.Contains(ball))
            {
                _pooledBalls.Add(ball);
            }

            Queue<BallController> pool = GetOrCreatePool(prefab);
            if (!pool.Contains(ball))
            {
                pool.Enqueue(ball);
            }

            ball.Deactivate();
        }
        
        public void ReleaseAll()
        {
            foreach (BallController ball in ActiveBalls.Reverse())
            {
                ReleaseBall(ball);
            }
        }
        
        #endregion
        
        
        BallController GetAvailableBall(GameObject prefab)
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
        
        Queue<BallController> GetOrCreatePool(GameObject prefab)
        {
            if (_poolsByPrefab.TryGetValue(prefab, out Queue<BallController> pool))
            {
                return pool;
            }

            pool = new Queue<BallController>();
            _poolsByPrefab[prefab] = pool;
            return pool;
        }

        #region Public Functions For Getting Data

        public BallController GetPooledBallOfType(EBallType eBallType)
        {
            foreach (BallController ball in _pooledBalls)
            {
                if (ball != null && ball.GetBallType() == eBallType)
                {
                    return ball;
                }
            }

            return null;
        }

        public int GetActiveBallCount(EBallType eBallType)
        {
            int count = 0;

            foreach (BallController ball in _activeBalls)
            {
                if (ball != null && ball.GetBallType() == eBallType)
                {
                    count++;
                }
            }

            return count;
        }

        #endregion
        


        


    }
}
