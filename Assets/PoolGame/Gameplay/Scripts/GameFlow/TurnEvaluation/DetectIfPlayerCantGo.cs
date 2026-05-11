using PoolGame.Gameplay.Attributes;
using PoolGame.Gameplay.Ball;
using UnityEngine;
namespace PoolGame.Gameplay.GameFlow.TurnEvaluation
{
	public class DetectIfPlayerCantGo : MonoBehaviour
	{
		[SerializeField] GameState state;
		[SerializeField] Life life;
		[SerializeField] Turn turn;
		[SerializeField] MovingBallsChecker movingBallsChecker;

		void Update()
		{
			if (state.Current != EGameState.InPlay)
				return;
			
			if (turn.GetAttributeValue() <= 0)
				return;
			
			if (movingBallsChecker.AreBallsMoving())
				return;

			if (life.GetAttributeValue() > 0)
				return;
			
			state.Set(EGameState.Finished);

		}
	}
}
