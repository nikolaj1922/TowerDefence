using Zenject;
using UnityEngine;

namespace _Project.Scripts.Weapons
{
    public class Weapon : MonoBehaviour
    {
        private WeaponLookToTarget _lookToTarget;

        public float DamageMultiplier { get; private set; } = 1f;
        public float AttackSpeedMultiplier { get; private set; } = 1f;
        
        [Inject]
        public void Construct(WeaponLookToTarget lookToTarget) => _lookToTarget = lookToTarget;

        private void Update() => _lookToTarget.Tick(Time.deltaTime);

        public void SetMultipliers(float damageMultiplier, float attaskSpeedMultiplier)
        {
            DamageMultiplier = damageMultiplier;
            AttackSpeedMultiplier =  attaskSpeedMultiplier;
        }
    }
}