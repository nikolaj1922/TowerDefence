using Zenject;
using UnityEngine;
using _Project.Scripts.Weapon;
using _Project.Scripts.Configs;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.Infrastructure.GameConstants;

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
        [SerializeField] private ParticleSystem _onAttackEffect;
        
        private WeaponConfigsRepository _configRepository;
        
        [Inject]
        private void Construct(WeaponConfigsRepository configRepository) => _configRepository = configRepository;
        
        public override void InstallBindings()
        {
            WeaponConfig config = _configRepository.Get(_weaponType);
            Container.Bind<WeaponConfig>().FromInstance(config).AsSingle();

            Container.Bind<Transform>().WithId(GameConstants.WEAPON_BASE_INJECT_ID).FromInstance(_weaponBase);
            Container.Bind<Transform>().WithId(GameConstants.WEAPON_HEAD_INJECT_ID).FromInstance(_weaponHead);
            Container.Bind<Transform>().WithId(GameConstants.PROJECTILE_POINT_INJECT_ID).FromInstance(_projectileSpawnPoint);
            
            Container.BindInterfacesAndSelfTo<WeaponTargetFinder>().AsSingle().WithArguments(_enemyLayerMask, transform.position);
            Container.Bind<WeaponAim>().AsSingle();
            Container.BindInterfacesAndSelfTo<WeaponAttack>().AsSingle()
                .WithArguments(_projectile, _onAttackEffect);
        }
    }
}