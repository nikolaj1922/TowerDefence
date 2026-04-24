using UnityEngine;
using Zenject;

namespace _Project.Scripts.UI.HealthBar
{
    public class LookToCamera : MonoBehaviour
    {
        private Camera _camera;

        [Inject]
        public void Construct(Camera mainCamera) => _camera = mainCamera;

        private void LateUpdate()
        {
            transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward,
                _camera.transform.rotation * Vector3.up);
        }
    }
}