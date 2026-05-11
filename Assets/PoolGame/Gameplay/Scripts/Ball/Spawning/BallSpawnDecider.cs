using PoolGame.Gameplay.GameFlow;
using UnityEngine;
using UnityEngine.Events;

namespace PoolGame.Gameplay.Ball.Spawning
{
	public class BallSpawnDecider : MonoBehaviour
	{
		[SerializeField] GameState gameState;
		[SerializeField] BallContainer ballContainer;

		[Header("Spawning")]
		public UnityEvent spawnObjectBallsCommand;
		public UnityEvent spawnCueBallCommand;

		bool _cueBallSpawnRequested;
		bool _objectBallsSpawnRequested;

		void Update()
		{
			if (gameState.Current != EGameState.InPlay)
				return;

			UpdateSpawnRequest(
				ballContainer.GetActiveBallCount(EBallType.CueBall),
				ref _cueBallSpawnRequested,
				spawnCueBallCommand);

			UpdateSpawnRequest(
				ballContainer.GetActiveBallCount(EBallType.ObjectBall),
				ref _objectBallsSpawnRequested,
				spawnObjectBallsCommand);
		}

		void UpdateSpawnRequest(int activeBallCount, ref bool spawnRequested, UnityEvent spawnCommand)
		{
			if (activeBallCount > 0)
			{
				spawnRequested = false;
				return;
			}

			if (spawnRequested)
				return;

			spawnRequested = true;
			spawnCommand?.Invoke();
		}
	}
}
