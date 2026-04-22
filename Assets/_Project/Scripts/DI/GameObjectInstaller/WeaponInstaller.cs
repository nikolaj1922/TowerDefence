using Zenject;
using UnityEngine;
using _Project.Scripts.Configs;
using _Project.Scripts.Database.Weapons;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Weapons;
using CartoonFX;

namespace _Project.Scripts.DI.GameObjectInstaller
{
    public class WeaponInstaller : MonoInstaller
    {
        [SerializeField] private Transform _weaponBase;
        [SerializeField] private Transform _weaponHead;
        [SerializeField] private Transform _projectileSpawnPoint;
        [SerializeField] private Weapon _weapon;
        [SerializeField] private LayerMask _enemyLayerMask;
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] private CFXR_Effect _onAttackEffect;
        [SerializeField] private WeaponProjectile _projectile;

        private WeaponDatabase _weaponDatabase;
        
        [Inject]
        private void Construct(WeaponDatabase weaponDatabase) => _weaponDatabase = weaponDatabase;
        
        public override void InstallBindings()
        {
            WeaponConfig config = _weaponDatabase.GetConfig(_weaponType);
            Container.Bind<WeaponConfig>().FromInstance(config).AsSingle();

            Container.Bind<Transform>().WithId(GameConstants.WEAPON_BASE_INJECT_ID).FromInstance(_weaponBase);
            Container.Bind<Transform>().WithId(GameConstants.WEAPON_HEAD_INJECT_ID).FromInstance(_weaponHead);
            Container.Bind<Transform>().WithId(GameConstants.PROJECTILE_POINT_INJECT_ID).FromInstance(_projectileSpawnPoint);
            
            Container.BindInterfacesAndSelfTo<WeaponTargetFinder>().AsSingle().WithArguments(_enemyLayerMask, transform.position);
            Container.Bind<WeaponLookToTarget>().AsSingle();
            Container.Bind<Weapon>().FromInstance(_weapon);
            Container.BindInterfacesAndSelfTo<WeaponAttackFX>().AsSingle().WithArguments(_onAttackEffect);
            Container.BindInterfacesAndSelfTo<WeaponAttack>().AsSingle()
                .WithArguments(_projectile);
        }
    }
}