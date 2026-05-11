using PoolGame.Gameplay.GameFlow;
using UnityEngine;
namespace PoolGame.Gameplay.Ball.Spawning
{
	public class DespawnBallsOnGameOver : MonoBehaviour
	{
		[SerializeField] GameState gameState;
		[SerializeField] BallContainer ballContainer;

		void OnEnable()
		{
			gameState.Changed += GameStateChanged;
		}
		
		void OnDisable()
		{
			gameState.Changed -= GameStateChanged;
		}

		void GameStateChanged(EGameState newState)
		{
			if(newState != EGameState.Finished)
				return;
			
			ballContainer.ReleaseAll();
		}
	}
}
