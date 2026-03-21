using Zenject;
using UnityEngine;
using UnityEngine.UI;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.SceneLoader;

namespace  _Project.Scripts.UI.CanvasMenu
{
    public class MenuCanvas : MonoBehaviour
    {
        private SceneLoader _sceneLoader;
        
        [Header("Buttons")]
        [SerializeField] private Button _goToLevelsMenuButton;
        
        private void Awake() =>  _goToLevelsMenuButton.onClick.AddListener(OnStartClick);

        private void OnDisable() => _goToLevelsMenuButton.onClick.RemoveListener(OnStartClick);

        [Inject]
        public void Construct(SceneLoader sceneLoader) => _sceneLoader = sceneLoader;

        private void OnStartClick() => _sceneLoader.LoadScene(GameConstants.LevelScene);
    }
}

