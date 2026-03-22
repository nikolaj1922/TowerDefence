using UnityEngine;
using Zenject;

namespace _Project.Scripts.Weapon
{
    public class Weapon : MonoBehaviour
    {
        private WeaponAim _aim;

        [Inject]
        public void Construct(WeaponAim aim) => _aim = aim;

        private void Update()
        {
            _aim.Tick(Time.deltaTime);
        }
    }
}