using PoolGame.Gameplay.Attributes;
using PoolGame.Gameplay.Ball;
using PoolGame.Gameplay.Pockets;
using UnityEngine;
namespace PoolGame.Gameplay.GameFlow.TurnEvaluation
{
	public class TurnPottedCueEvaluator : MonoBehaviour
	{
		[SerializeField] Life life;
		
		void OnEnable()
		{
			PocketController.OnBallPocketed += OnBallPocketed;
		}
        
		void OnDisable()
		{
			PocketController.OnBallPocketed -= OnBallPocketed;
		}
		
		void OnBallPocketed(BallController ball, PocketController pocket)
		{
			if (ball == null)
				return;
	        
			if(ball.GetBallType() != EBallType.CueBall)
				return;

			OnCueBallPocketed();
		}
		
		void OnCueBallPocketed()
		{
			life.DecreaseAttribute(1);
		}
	}
}
