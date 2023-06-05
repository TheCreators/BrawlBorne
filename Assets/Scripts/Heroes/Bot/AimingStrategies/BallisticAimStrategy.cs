using Combat;
using Combat.Ultimates;
using Combat.Weapons;
using UnityEngine;

namespace Heroes.Bot.AimingStrategies
{
    [CreateAssetMenu(menuName = "Aiming Strategies/Ballistic")]
    public class BallisticAimStrategy : AimingStrategy
    {
        public override Quaternion GetAimRotation(Vector3 attackerPosition, Vector3 targetPosition, Usable usable)
        {
            BallisticGun ballisticGun;

            if (usable is BallisticGun gun1)
            {
                ballisticGun = gun1;
            }
            else if (usable is WeaponUpgradeUltimate {Weapon: BallisticGun gun2})
            {
                ballisticGun = gun2;
            }
            else
            {
                Debug.LogError("BallisticAimStrategy can only be used with BallisticGun");
                return Quaternion.identity;
            }

            Vector3 direction = GetDirection(attackerPosition, targetPosition, ballisticGun);
            return Quaternion.LookRotation(direction);
        }

        private static Vector3 GetDirection(Vector3 attackerPosition, Vector3 targetPosition, BallisticGun ballisticGun)
        {
            // Calculate the vector from the attacker to the target
            Vector3 toTarget = targetPosition - attackerPosition;

            // Calculate the horizontal distance to the target
            float horizontalDistance = new Vector3(toTarget.x, 0, toTarget.z).magnitude;

            // Calculate the vertical distance to the target
            float verticalDistance = toTarget.y;

            // Calculate the initial speed of the projectile
            float initialSpeed = ballisticGun.ThrowPower;

            // Calculate the acceleration due to gravity
            float gravity = Physics.gravity.magnitude;

            // Calculate the square of the initial speed
            float initialSpeedSq = initialSpeed * initialSpeed;

            // Calculate the determinant of the quadratic equation for the launch angle
            float determinant = initialSpeedSq * initialSpeedSq -
                                gravity * (gravity * horizontalDistance * horizontalDistance + 2 * verticalDistance * initialSpeedSq);

            // Calculate the launch angle
            float angle;

            if (determinant >= 0)
            {
                // If the target is within range, calculate the two possible launch angles and choose the larger one
                float angle1 = Mathf.Atan((initialSpeedSq + Mathf.Sqrt(determinant)) / (gravity * horizontalDistance));
                float angle2 = Mathf.Atan((initialSpeedSq - Mathf.Sqrt(determinant)) / (gravity * horizontalDistance));
                angle = Mathf.Max(angle1, angle2);
            }
            else
            {
                // If the target is out of range, use a launch angle of 45 degrees for maximum range
                angle = Mathf.PI / 4;
            }

            // Calculate the direction of launch
            return new Vector3(toTarget.x, Mathf.Tan(angle) * horizontalDistance, toTarget.z).normalized;
        }
    }
}