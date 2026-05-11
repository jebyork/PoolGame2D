using UnityEngine;

namespace PoolGame.Gameplay.Ball
{
    public class MovingBallsChecker : MonoBehaviour
    {
        [SerializeField] BallContainer ballContainer;
        
        public bool AreBallsMoving()
        {
	        foreach (BallController ball in ballContainer.ActiveBalls)
	        {
		        if (ball.IsMovingAboveSpeed(0))
		        {
			        return true;
		        }
	        }
	        return false;
        }
    }
}
