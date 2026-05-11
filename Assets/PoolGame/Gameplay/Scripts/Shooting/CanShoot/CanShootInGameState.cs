using PoolGame.Gameplay.GameFlow;
using UnityEngine;
namespace PoolGame.Gameplay.Shooting.CanShoot
{
	[CreateAssetMenu(fileName = "Can Shoot Game In State", menuName = "Shooting/Can Shoot/GameSate", order = 0)]
	public class CanShootInGameState : CanShootStrategy
	{
		[SerializeField] EGameState stateCanShootIn;
		
		GameState _state;


		public override bool CanShoot(PlayerShootingController shotRequester)
		{
			if (!_state)
			{
				_state = FindFirstObjectByType<GameState>();
				if(!_state)
					return false;
			}

			if (_state.Current == stateCanShootIn)
				return true;
			
			Debug.Log("Cant Shoot not in the right state");
			return false;
		}
	}
}
