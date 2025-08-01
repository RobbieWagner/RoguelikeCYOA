using RobbieWagnerGames.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RobbieWagnerGames.FirstPerson
{
    /// <summary>
    /// Handles first-person camera rotation with mouse and controller support
    /// </summary>
    public class FirstPersonLook : MonoBehaviour
    {
        [Header("Look Settings")]
        [SerializeField] private float mouseSensitivity = 500f;
        [SerializeField] private float controllerSensitivity = 0.5f;
        [SerializeField] private Transform playerBody;
        [SerializeField] private bool lockCursorOnStart = true;

        [Header("View Constraints")]
        [SerializeField] private float minVerticalAngle = -90f;
        [SerializeField] private float maxVerticalAngle = 90f;

        private float xRotation = 0f;
        private Vector2 currentLookInput = Vector2.zero;
        private InputAction lookAction;
        private bool isUsingController = false;

        private bool canLook = true;
        public bool CanLook
        {
            get => canLook;
            set
            {
                if (value == canLook) return;
                
                canLook = value;
                UpdateCursorState();
                onLookStateChanged?.Invoke(canLook);
            }
        }

        public delegate void LookStateChanged(bool canLook);
        public event LookStateChanged onLookStateChanged;

        private void Awake()
        {
            InitializeCursor();
            SetupInput();
        }

        private void Update()
        {
            if (!CanLook) return;

            ProcessLookInput();
        }

        private void InitializeCursor()
        {
            Cursor.lockState = lockCursorOnStart ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !lockCursorOnStart;
        }

        private void SetupInput()
        {
            lookAction = InputManager.Instance.GetAction(ActionMapName.EXPLORATION , "EXPLORATION/Look");
            lookAction.performed += OnLookPerformed;
            lookAction.canceled += OnLookCanceled;
        }

        private void ProcessLookInput()
        {
            if (currentLookInput == Vector2.zero) return;

            float sensitivity = isUsingController ? controllerSensitivity : mouseSensitivity;
            Vector2 scaledInput = currentLookInput * sensitivity * Time.deltaTime;

            // Vertical rotation (up/down)
            xRotation -= scaledInput.y;
            xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // Horizontal rotation (left/right)
            playerBody.Rotate(Vector3.up * scaledInput.x);
        }

        private void OnLookPerformed(InputAction.CallbackContext context)
        {
            currentLookInput = context.ReadValue<Vector2>();
            isUsingController = context.control.device is Gamepad;
        }

        private void OnLookCanceled(InputAction.CallbackContext context)
        {
            currentLookInput = Vector2.zero;
        }

        private void UpdateCursorState()
        {
            if (CanLook)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        private void OnDestroy()
        {
            if (lookAction != null)
            {
                lookAction.performed -= OnLookPerformed;
                lookAction.canceled -= OnLookCanceled;
            }
        }
    }
}