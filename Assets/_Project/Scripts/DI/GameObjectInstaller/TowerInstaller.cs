using Zenject;
using UnityEngine;
using _Project.Scripts.Tower;
using _Project.Scripts.Configs;
using _Project.Scripts.Repositories;
using _Project.Scripts.Tower.Weapon;

namespace _Project.Scripts.DI.GameObjectInstaller
{
    public class TowerInstaller : MonoInstaller
    {
        [SerializeField] private TowerType _towerType;
        [SerializeField] private LayerMask _enemyLayerMask;
        [SerializeField] private Transform _weaponBase;
        [SerializeField] private Transform _weaponHead;
        [SerializeField] private Transform _projectileSpawnPoint;
        [SerializeField] private WeaponProjectile _projectile;
        
        private TowersRepository _configsRepository;
        
        [Inject]
        public void Construct(TowersRepository repository) => _configsRepository = repository;
        
        public override void InstallBindings()
        {
            Container.Bind<Transform>().WithId("WeaponBase").FromInstance(_weaponBase);
            Container.Bind<Transform>().WithId("WeaponHead").FromInstance(_weaponHead);
            Container.Bind<Transform>().WithId("ProjectileSpawnPoint").FromInstance(_projectileSpawnPoint);
            
            TowerConfig config = _configsRepository.Get(_towerType);
            Container.Bind<TowerConfig>().FromInstance(config).AsSingle();
            
            Container.Bind<WeaponTargetFinder1>().AsSingle().WithArguments(_enemyLayerMask, config, transform.position);
            Container.Bind<WeaponAim1>().AsSingle().WithArguments(config);
            Container.BindInterfacesAndSelfTo<WeaponAttack1>().AsSingle()
                .WithArguments(_projectile);
        }
    }
}
