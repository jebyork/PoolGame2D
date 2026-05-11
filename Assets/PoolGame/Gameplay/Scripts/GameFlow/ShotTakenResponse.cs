using PoolGame.Gameplay.Attributes;
using PoolGame.Gameplay.Shooting;
using UnityEngine;
namespace PoolGame.Gameplay.GameFlow
{
	public class ShotTakenResponse : MonoBehaviour
	{
		[SerializeField] Life life;
		[SerializeField] Turn turn;
		[SerializeField] PlayerShootingController playerShootingController;

		void OnEnable()
		{
			playerShootingController.OnShotTaken += ShotTaken;
		}

		void OnDisable()
		{
			playerShootingController.OnShotTaken -= ShotTaken;
		}

		void ShotTaken()
		{
			life.DecreaseAttribute(1);
			turn.IncreaseAttribute(1);
		}
	}
}
