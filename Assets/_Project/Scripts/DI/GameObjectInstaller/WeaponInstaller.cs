using Zenject;
using UnityEngine;
using _Project.Scripts.Weapon;
using _Project.Scripts.Configs;
using _Project.Scripts.Repositories;

namespace _Project.Scripts.DI.GameObjectInstaller
{
    public class WeaponInstaller : MonoInstaller
    {
        [SerializeField] private Transform _weaponBase;
        [SerializeField] private Transform _weaponHead;
        [SerializeField] private Transform _projectileSpawnPoint;
        [SerializeField] private WeaponProjectile _projectile;
        [SerializeField] private LayerMask _enemyLayerMask;
        [SerializeField] private WeaponType _weaponType;
        
        private WeaponConfigsRepository _configRepository;
        
        [Inject]
        private void Construct(WeaponConfigsRepository configRepository) => _configRepository = configRepository;
        
        public override void InstallBindings()
        {
            WeaponConfig config = _configRepository.Get(_weaponType);

            Container.Bind<WeaponConfig>().FromInstance(config).AsSingle();
            
            Container.Bind<Transform>().WithId("WeaponBase").FromInstance(_weaponBase);
            Container.Bind<Transform>().WithId("WeaponHead").FromInstance(_weaponHead);
            Container.Bind<Transform>().WithId("ProjectileSpawnPoint").FromInstance(_projectileSpawnPoint);
            
            Container.Bind<WeaponTargetFinder>().AsSingle().WithArguments(_enemyLayerMask, transform.position);
            Container.Bind<WeaponAim>().AsSingle();
            Container.BindInterfacesAndSelfTo<WeaponAttack>().AsSingle()
                .WithArguments(_projectile);
        }
    }
}