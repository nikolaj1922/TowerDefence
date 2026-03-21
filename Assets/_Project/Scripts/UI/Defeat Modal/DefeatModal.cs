using System;
using Zenject;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.SceneLoader;

public class DefeatModal : MonoBehaviour, IInitializable, IDisposable
{
    private SceneLoader _sceneLoader;
    
    [SerializeField] private Button _tryAgainButton;
    [SerializeField] private Button _goToMenuButton;

    [Inject]
    public void Construct(SceneLoader sceneLoader) => _sceneLoader = sceneLoader;

    public void Initialize()
    {
        _tryAgainButton.onClick.AddListener(OnTryAgainButtonClick);
        _goToMenuButton.onClick.AddListener(OnGoToMenuButtonClick);
    }

    public void Dispose()
    {
        _tryAgainButton.onClick.RemoveListener(OnTryAgainButtonClick);
        _goToMenuButton.onClick.RemoveListener(OnGoToMenuButtonClick);
    }

    private void OnTryAgainButtonClick() => _sceneLoader.LoadScene(SceneManager.GetActiveScene().name);
    
    private void OnGoToMenuButtonClick() => _sceneLoader.LoadScene(GameConstants.MenuScene);
}
