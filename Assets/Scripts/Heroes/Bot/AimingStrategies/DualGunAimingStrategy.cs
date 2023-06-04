using Combat;
using Combat.Ultimates;
using Combat.Weapons;
using UnityEngine;

namespace Heroes.Bot.AimingStrategies
{
    [CreateAssetMenu(menuName = "Aiming Strategies/Dual Gun")]
    public class DualGunAimingStrategy : AimingStrategy
    {
        public override Quaternion GetAimRotation(Vector3 attackerPosition, Vector3 targetPosition, Usable usable)
        {
            DualGun dualGun;
            
            if (usable is DualGun gun1)
            {
                dualGun = gun1;
            }
            else if (usable is WeaponUpgradeUltimate {Weapon: DualGun gun2})
            {
                dualGun = gun2;
            }
            else
            {
                Debug.LogError("DualGunAimingStrategy can only be used with DualGun");
                return Quaternion.identity;
            }

            // Calculate spread direction (right or left)
            Vector3 spreadDirection = !dualGun.ShotFromRight ? dualGun.transform.right : -dualGun.transform.right;
            Vector3 spreadVector = spreadDirection * dualGun.BulletsSpread;

            // Adjust aimDirection with spread
            Vector3 aimDirection = targetPosition + spreadVector - attackerPosition;
        
            return Quaternion.LookRotation(aimDirection, Vector3.up);
        }
    }
}