using Combat;
using UnityEngine;

namespace Heroes.Bot.AimingStrategies
{
    public abstract class AimingStrategy : ScriptableObject
    {
        public abstract Quaternion GetAimRotation(Vector3 attackerPosition, Vector3 targetPosition, Usable usable);
    }
}