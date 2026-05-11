using PoolGame.Game.Attribute;
using UnityEngine;
namespace PoolGame.Gameplay.GameFlow.TurnEvaluation
{
    public class TurnModifiers : MonoBehaviour
    {
        [Header("Score")]
        public ModifiableStat scorePerObjectBall;

        
        // Unused For Now
        [Header("Life")]
        public ModifiableStat lifePunishment;

        public void UpdateTurn()
        {
            scorePerObjectBall.Update();
            lifePunishment.Update();
        }
    }
}
