// using System;
// using System.Linq;
// using PoolGame.Gameplay.Ball.Events;
// using UnityEngine;
//
// namespace PoolGame.Gameplay.Ball
// {
//     public class MovingBallsChecker : MonoBehaviour
//     {
//         private bool _ballsAreMoving = false;
//
//         [SerializeField] private BallManager ballManager;
//         [SerializeField] private BallsStateChangeChannel stateChangeEventChannel;
//
//         private void OnValidate()
//         {
//             if (ballManager == null)
//             {
//                 Debug.LogError("[MovingBallsChecker] Ball manager is null");
//             }
//         }
//
//         private void Update()
//         {
//             if (ballManager == null) 
//                 return;
//
//             bool anyMovingNow = ballManager.BallControllers
//                 .Any(b => b.IsMoving);
//
//
//             switch (_ballsAreMoving)
//             {
//                 case false when anyMovingNow:
//                     _ballsAreMoving = true;
//                     stateChangeEventChannel?.RaiseEvent(BallState.Moving);
//                     break;
//                 case true when !anyMovingNow:
//                     _ballsAreMoving = false;
//                     stateChangeEventChannel?.RaiseEvent(BallState.Stopped);
//                     break;
//             }
//         }
//     }
// }