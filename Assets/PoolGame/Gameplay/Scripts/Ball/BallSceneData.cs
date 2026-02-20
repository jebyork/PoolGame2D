using UnityEngine;

namespace PoolGame.Gameplay.Ball
{
    [CreateAssetMenu(fileName = "Ball Scene Data" , menuName = "Balls/Scene Data" , order = 0)]
    public class BallSceneData : ScriptableObject
    {
        public int MaxNumberOfBalls;
        public int InitialNumberOfBalls;
        public GameObject BallPrefab;
    }
}
