using _Project.Scripts.Enemy;
using Zenject;
using _Project.Scripts.Level;
using _Project.Scripts.Tower;
using UnityEngine;

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

            Container.Bind<CastleController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<EnemySpawner>().AsSingle().WithArguments(viewWidth, viewHeight);
            Container.Bind<EnemyFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle().WithArguments(_defeatModal);
        }
    }
}