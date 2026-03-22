using _Project.Scripts.Database.EnemyDatabase;
using Zenject;
using UnityEngine;
using _Project.Scripts.Enemy;
using _Project.Scripts.Level;
using _Project.Scripts.Tower.Castle;

namespace _Project.Scripts.DI.SceneContext.LevelScene
{
    public class LevelSceneInstaller : MonoInstaller
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private DefeatModal _defeatModal;

        private EnemyPrefabsDatabase _enemyPrefabsDatabase;
        
        [Inject]
        public void Construct(EnemyPrefabsDatabase enemyPrefabsDatabase )
        {
            _enemyPrefabsDatabase = enemyPrefabsDatabase;
        }
        
        public override void InstallBindings()
        {
            float viewHeight = _camera.orthographicSize * 2;
            float viewWidth = viewHeight * _camera.aspect;

            Container.BindMemoryPool<EnemyController, EnemyPool>().AsSingle();
            
            Container.Bind<EnemySpawner>().AsSingle().WithArguments(viewWidth, viewHeight);
            Container.Bind<EnemyFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle().WithArguments(_defeatModal);
        }
    }
}