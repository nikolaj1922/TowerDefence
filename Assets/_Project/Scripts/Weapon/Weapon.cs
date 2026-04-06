using UnityEngine;
using Zenject;

namespace _Project.Scripts.Weapon
{
    public class Weapon : MonoBehaviour
    {
        private WeaponLookToTarget _lookToTarget;

        [Inject]
        public void Construct(WeaponLookToTarget lookToTarget) => _lookToTarget = lookToTarget;

        private void Update() => _lookToTarget.Tick(Time.deltaTime);
    }
}