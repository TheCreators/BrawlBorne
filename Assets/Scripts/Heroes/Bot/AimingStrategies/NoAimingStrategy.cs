using Combat;
using UnityEngine;

namespace Heroes.Bot.AimingStrategies
{
    [CreateAssetMenu(menuName = "Aiming Strategies/No Aiming")]
    public class NoAimingStrategy : AimingStrategy
    {
        public override Quaternion GetAimRotation(Vector3 attackerPosition, Vector3 targetPosition, Usable usable)
        {
            return Quaternion.identity;
        }
    }
}