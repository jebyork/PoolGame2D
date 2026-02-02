using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PoolGame.Core.Game.States.Gameplay.Table
{
    public class WallController : MonoBehaviour
    {
        [SerializeField] private GameObject pocketPrefab;
        [SerializeField] private float pocketMaxOutsideOffset;
        [SerializeField] private float pocketMaxInsideOffset;
        
        [SerializeField] private float pocketMinDistance;
        [SerializeField] private float pocketMaxDistance;
        
        [SerializeField] private float wallRotation;
        [SerializeField] private int initialPocketCount = 6;
        [SerializeField] private int maxPocketsActive;

        private float PocketRadius => pocketPrefab.GetComponent<PocketController>().Radius;

        private readonly List<GameObject> _pockets = new();

        private TableSide _tableSide;

        private float _currentYPos = 0f;
        
        public void CreateInitialPockets()
        {
            _pockets.Clear();
            for (int i = 0; i < initialPocketCount; i++)
            {
                GameObject pocket = Instantiate(pocketPrefab, transform);
                _pockets.Add(pocket);
            }
        }
        
        public void SetSide(TableSide side) => _tableSide = side;

        public void SetInitialPocketPositions()
        {
            Vector3 startPos = transform.position;
            _currentYPos = 0f;

            foreach (GameObject pocket in _pockets)
            {
                float gap = GetRandomGap();
                float offset = GetRandomOffset();
                _currentYPos += GetGapWithPocketRadius(gap);
                pocket.transform.position = GetNextPocketPosition(_currentYPos, offset, startPos);
            }
        }
        
        private Vector3 GetNextPocketPosition(float yPos, float offset, Vector3 startPos)
        {
            return startPos + new Vector3(offset, yPos, 0f);
        }

        private float GetRandomGap()
        {
            return Random.Range(pocketMinDistance, pocketMaxDistance);
        }

        private float GetRandomOffset()
        {
            return Random.Range(-pocketMaxOutsideOffset, pocketMaxInsideOffset);
        }

        private float GetGapWithPocketRadius(float gap)
        {
            return PocketRadius * 2f + gap;
        }
    }

    [Serializable]
    public enum TableSide
    {
        Left,
        Right,
    }
}
