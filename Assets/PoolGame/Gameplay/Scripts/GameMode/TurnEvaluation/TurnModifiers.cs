using PoolGame.Game.Attribute;
using UnityEngine;

namespace PoolGame.Gameplay.GameMode.TurnEvaluation
{
    public class TurnModifiers : MonoBehaviour
    {
        [Header("Score")]
        public ModifiableStat scorePerObjectBall;

        [Header("Life")]
        public ModifiableStat lifePunishment;

        public void UpdateTurn()
        {
            scorePerObjectBall.Update();
            lifePunishment.Update();
        }
    }
}
