using System;
using PoolGame.Gameplay.Attributes;
using UnityEngine;
namespace PoolGame.Gameplay.GameFlow
{
	public class GameOverDecider : MonoBehaviour
	{
		[SerializeField] GameState state;
		[SerializeField] Life life;

		void OnEnable()
		{
			life.OnNoLife += NoLife;
		}

		void OnDisable()
		{
			life.OnNoLife -= NoLife;
		}

		void NoLife()
		{
			state.Set(EGameState.Finished);
		}
	}
}
