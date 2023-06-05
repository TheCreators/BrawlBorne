using Combat;
using UnityEngine;

namespace Heroes.Bot.AimingStrategies
{
    [CreateAssetMenu(menuName = "Aiming Strategies/Direct")]
    public class DirectAimingStrategy : AimingStrategy
    {
        public override Quaternion GetAimRotation(Vector3 attackerPosition, Vector3 targetPosition, Usable usable)
        {
            Vector3 aimDirection = targetPosition - attackerPosition;
            return Quaternion.LookRotation(aimDirection, Vector3.up);
        }
    }
}