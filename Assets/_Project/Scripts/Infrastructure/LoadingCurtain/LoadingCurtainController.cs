using System;
using Zenject;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure.LoadingCurtain.Operations;

namespace _Project.Scripts.Infrastructure.LoadingCurtain
{
    public class LoadingCurtainController: IInitializable, IDisposable
    {
        private readonly LoadingCurtainView _view;
        private readonly LoadingCurtainModel _model;
        
        public LoadingCurtainController(LoadingCurtainView view, LoadingCurtainModel model)
        {
            _view = view;
            _model = model;
        }

        public async UniTask Run(Queue<ILoadingOperation> loadingOperations)
        {
            _view.gameObject.SetActive(true);
            
            _view.ResetLoadingProgress();
            await _model.Run(loadingOperations);
            
            _view.gameObject.SetActive(false);
        }

        public void Initialize()
        {
            _model.OnNewLoadingOperationStart += _view.SetDescription;
            _model.OnLoadingOperationProgress += _view.SetLoadingProgress;
        }

        public void Dispose()
        {
            _model.OnNewLoadingOperationStart -= _view.SetDescription;
            _model.OnLoadingOperationProgress -= _view.SetLoadingProgress;
        }
    }
}