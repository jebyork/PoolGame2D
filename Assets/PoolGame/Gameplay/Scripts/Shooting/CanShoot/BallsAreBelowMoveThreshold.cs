using PoolGame.Gameplay.Ball;
using UnityEngine;
namespace PoolGame.Gameplay.Shooting.CanShoot
{
	
	[CreateAssetMenu(fileName = "Balls Speed Below Shot Threshold", menuName = "Shooting/Can Shoot/Speed Threshold", order = 0)]
	public class BallsAreBelowMoveThreshold : CanShootStrategy
	{
		[SerializeField] float canShootBelowSpeed;
		
		BallContainer _ballContainer;
		
		public override bool CanShoot(PlayerShootingController shotRequester)
		{
			if (!_ballContainer)
			{
				_ballContainer = FindFirstObjectByType<BallContainer>();
				if(!_ballContainer)
					return false;
			}

			foreach (BallController ball in _ballContainer.ActiveBalls)
			{
				if (!ball.IsMovingAboveSpeed(canShootBelowSpeed))
					continue;
				Debug.Log("Can't shoot as ball is moving too fast");
				return false;
			}
			
			return true;
		}
	}
}
