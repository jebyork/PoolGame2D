using System;
using PoolGame.Gameplay.Attributes;
using PoolGame.Gameplay.Ball;
using PoolGame.Gameplay.Pockets;
using UnityEngine;
namespace PoolGame.Gameplay.GameFlow.TurnEvaluation
{
    public class TurnPottedObjectBallEvaluator : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] GameState state;
        [SerializeField] TurnModifiers turnModifiers;
        [SerializeField] Life life;
		[SerializeField] Score score;
        [SerializeField] Turn turn;
        
        [Header("Effects")]
        [SerializeField] bool lifePerBall = false;

        int _objectBallsPottedThisTurn = 0;

        #region Lifecycle 

        void OnEnable()
        {
            PocketController.OnBallPocketed += OnBallPocketed;
            turn.OnAttributeChanged += OnNewTurn;
        }
        
        void OnDisable()
        {
            PocketController.OnBallPocketed -= OnBallPocketed;
            turn.OnAttributeChanged -= OnNewTurn;
        }
        
        #endregion
        
        void OnNewTurn(int turnNumber)
        {
	        _objectBallsPottedThisTurn = 0;
	        turnModifiers.UpdateTurn();
        }
        
        void OnBallPocketed(BallController ball, PocketController pocket)
        {
	        if (ball == null)
		        return;
	        
	        if(ball.GetBallType() != EBallType.ObjectBall)
		        return;
	        
	        PottedObjectBall(); 
        }

        void PottedObjectBall()
        {
	        _objectBallsPottedThisTurn++;
	        DecideWhetherToGiveLife();

	        int value = turnModifiers.scorePerObjectBall.Value * _objectBallsPottedThisTurn;
	        score.IncreaseAttribute(value);
        }

        void DecideWhetherToGiveLife()
        {
	        if (!lifePerBall && _objectBallsPottedThisTurn != 1)
		        return;

	        life.IncreaseAttribute(1);
        }
    }
}
