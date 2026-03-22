using Zenject;
using UnityEngine;
using _Project.Scripts.Enemy;
using _Project.Scripts.Level;
using _Project.Scripts.Tower;
using _Project.Scripts.Weapon;

namespace _Project.Scripts.DI.SceneContext.LevelScene
{
    public class LevelSceneInstaller : MonoInstaller
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private DefeatModal _defeatModal;
        
        public override void InstallBindings()
        {
            float viewHeight = _camera.orthographicSize * 2;
            float viewWidth = viewHeight * _camera.aspect;

            Container.BindMemoryPool<Enemy.Enemy, EnemyPool>().AsSingle();
            
            Container.Bind<EnemySpawner>().AsSingle().WithArguments(viewWidth, viewHeight);
            Container.Bind<EnemyFactory>().AsSingle();
            Container.Bind<TowerFactory>().AsSingle();
            Container.Bind<WeaponFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle().WithArguments(_defeatModal);
        }
    }
}