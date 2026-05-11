using PoolGame.Gameplay.Shooting.Aiming;
using UnityEngine;

namespace PoolGame.Gameplay.Shooting.CanShoot
{
    [CreateAssetMenu(fileName = "Has Shootable To Shoot", menuName = "Shooting/Can Shoot/HasShootable", order = 0)]
    public class HasShootableToShoot : CanShootStrategy
    {
        public override bool CanShoot(PlayerShootingController shotRequester)
        {
	        if (shotRequester.GetShootable() != null) 
		        return true;
	        
	        Debug.Log("Cant Shoot no shootable object found");
            return false;
        }
    }
}