using Zenject;
using UnityEngine;
using UnityEngine.UI;
using _Project.Scripts.Infrastructure.GameConstants;
using _Project.Scripts.Infrastructure.SceneLoader;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.UI.MainMenu
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

        private void OnStartClick() => _sceneLoader.LoadScene(GameConstants.LEVEL_SCENE).Forget();
    }
}

