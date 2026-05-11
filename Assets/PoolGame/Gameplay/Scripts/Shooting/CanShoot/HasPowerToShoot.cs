using PoolGame.Gameplay.Shooting.Aiming;
using UnityEngine;

namespace PoolGame.Gameplay.Shooting.CanShoot
{
    [CreateAssetMenu(fileName = "Shot Has Power", menuName = "Shooting/Can Shoot/HasPower", order = 0)]
    public class HasPowerToShoot : CanShootStrategy
    {
        [SerializeField, Range(0f, 1f)] float minPower;

        public override bool CanShoot(PlayerShootingController shotRequester)
        {
	        if (shotRequester.GetAimPower() >= minPower)
		        return true;
	        
	        Debug.Log("Cant Shoot not enough power");
	        return false;
        }
    }
}