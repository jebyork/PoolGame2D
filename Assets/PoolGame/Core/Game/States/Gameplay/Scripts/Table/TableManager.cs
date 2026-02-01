using System.Collections.Generic;
using UnityEngine;

namespace PoolGame.Core.Game.States.Gameplay.Table
{
    public class TableManager : MonoBehaviour
    {
        [SerializeField] private WallController wallControllerPrefab;
        [SerializeField] private float tableWidth;
        
        private List<WallController> _wallControllers = new ();
        private readonly float _numberOfWalls = 2;

        public void CreateTable()
        {
            _wallControllers.Clear();

            for (int i = 0; i < _numberOfWalls; i++)
            {
                WallController wallController = Instantiate(wallControllerPrefab, transform);

                TableSide side = i == 0 ? TableSide.Left : TableSide.Right;
                wallController.SetSide(side);

                wallController.CreateInitialPockets();
                _wallControllers.Add(wallController);
            }
        }
        
        public void SetTableActiveness(bool active)
        {
            foreach (WallController wallController in _wallControllers)
            {
                wallController.gameObject.SetActive(active);
            }
        }

        public void SetTablePosition()
        {
            if (_wallControllers == null || _wallControllers.Count < 2) 
                return;

            _wallControllers[0].transform.position =
                SetWallX(_wallControllers[0].transform.position, -tableWidth);
            _wallControllers[0].SetInitialPocketPositions();

            _wallControllers[1].transform.position =
                SetWallX(_wallControllers[1].transform.position, tableWidth);
            _wallControllers[1].SetInitialPocketPositions();
        }
        
        private Vector3 SetWallX(Vector3 position, float x)
        {
            position.x = x;
            return position;
        }
    }
    
}
