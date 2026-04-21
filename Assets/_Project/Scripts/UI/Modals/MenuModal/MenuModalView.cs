using System;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Services.SceneLoader;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.UI.Modals.MenuModal
{
    public class MenuModalView : MonoBehaviour
    {
        public event Action OnStartClicked;
        public event Action OnOpenUpgradesClicked;
        
        [Header("Buttons")]
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _upgradeButton;
        
        private ISceneLoader _sceneLoader;
        private AssetReference _levelAssetReference;

        [Inject]
        public void Construct(
            ISceneLoader sceneLoader,
            [Inject(Id = GameConstants.LEVEL_SCENE)] AssetReference levelAssetReference)
        {
            _levelAssetReference = levelAssetReference;
            _sceneLoader = sceneLoader;
        }
        
        private void Awake()
        {
            _startButton.onClick.AddListener(OnStartButtonClicked);
            _upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        }

        private void Start() => _sceneLoader.Preload(_levelAssetReference.RuntimeKey.ToString());

        private void OnDestroy()
        {
            _startButton.onClick.RemoveListener(OnStartButtonClicked);
            _upgradeButton.onClick.RemoveListener(OnUpgradeButtonClicked);
        }
        
        private void OnStartButtonClicked() => OnStartClicked?.Invoke();
        private void OnUpgradeButtonClicked() => OnOpenUpgradesClicked?.Invoke();
    }
}

