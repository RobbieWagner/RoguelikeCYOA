using RobbieWagnerGames.Utilities;
using UnityEngine;

namespace RobbieWagnerGames.TileSelectionGame
{
    public class CameraManager : MonoBehaviourSingleton<CameraManager>
    {
        [SerializeField] private Camera _gameCamera;
        [SerializeField] private Vector3 _cameraPosOffset;

        public Camera GameCamera => _gameCamera;

        protected override void Awake()
        {
            if (Instance != null)
                _gameCamera.gameObject.SetActive(false);

            base.Awake();
            
            if (_gameCamera == null)
            {
                _gameCamera = Camera.main;
            }
        }

        public void SetGameCamera(Camera camera)
        {
            _gameCamera.gameObject.SetActive(false);
            _gameCamera = camera;
            camera.gameObject.SetActive(true);
        }
    }
}