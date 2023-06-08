using Misc;
using Models;
using NaughtyAttributes;
using UnityEngine;

namespace Heroes
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] [BoxGroup(Group.Settings)] [Required]
        private Transform _shootAt;
        
        [SerializeField] [BoxGroup(Group.Settings)] [Required]
        private Transform _shootFrom;

        [SerializeField] [BoxGroup(Group.Settings)]
        private string _name;

        public Vector3 ShootAt => _shootAt.position;
        
        public Vector3 ShootFrom => _shootFrom.position;

        public string Name => _name;

        protected virtual void OnValidate()
        {
            this.CheckIfNull(_shootAt);
            this.CheckIfNull(_name);
        }
    }
}