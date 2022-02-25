using Input;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform _playerCamera;
        [SerializeField] private float _mouseSensitivity = 3.5f;
        [SerializeField] private float _walkSpeed = 6f;
        [SerializeField] private float _sprintSpeed = 12f;
        [SerializeField] private float _gravity = -13f;
        [SerializeField, Range(0f, 5f)] private float _moveSmoothTime = 0.3f;
        [SerializeField, Range(0f, 5f)] private float _mouseSmoothTime = 0.03f;
        [SerializeField] private bool _lockCursor = true;

        private bool _isSprinting = false;

        private float _cameraPitch = 0f;
        private float _velocityY = 0f;

        private Vector2 _currentDirection = Vector2.zero;
        private Vector2 _currentDirectionVelocity = Vector2.zero;

        private Vector2 _currentMouseDelta = Vector2.zero;
        private Vector2 _currentMouseDeltaVelocity = Vector2.zero;

        private Controls _inputActions;
        private CharacterController _characterController;

        private void Awake()
        {
            _inputActions = InputManager.InputActions;
            _characterController = GetComponent<CharacterController>();

            if (_lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void OnEnable()
        {
            _inputActions.Player.Enable();
        }

        private void Update()
        {
            if (_inputActions.Player.enabled)
            {
                _isSprinting = _inputActions.Player.Sprint.activeControl != null;

                UpdateMouseLook();
                UpdateMovement();
            }
        }

        private void UpdateMouseLook()
        {
            var mouseInput = _inputActions.Player.Look.ReadValue<Vector2>();

            var targetMouseDelta = new Vector2(mouseInput.x, mouseInput.y);

            _currentMouseDelta = Vector2.SmoothDamp(_currentMouseDelta, targetMouseDelta, ref _currentMouseDeltaVelocity, _mouseSmoothTime);

            _cameraPitch -= _currentMouseDelta.y * _mouseSensitivity;
            _cameraPitch = Mathf.Clamp(_cameraPitch, -90f, 90f);

            _playerCamera.localEulerAngles = Vector3.right * _cameraPitch;

            transform.Rotate(Vector3.up * _currentMouseDelta.x * _mouseSensitivity);
        }

        private void UpdateMovement()
        {
            var inputVector = _inputActions.Player.Move.ReadValue<Vector2>();

            var targetDirection = new Vector2(inputVector.x, inputVector.y);

            targetDirection.Normalize();

            _currentDirection = Vector2.SmoothDamp(_currentDirection, targetDirection, ref _currentDirectionVelocity, _moveSmoothTime);

            if (_characterController.isGrounded)
            {
                _velocityY = 0f;
            }

            _velocityY += _gravity * Time.deltaTime;

            var speed = _isSprinting ? _sprintSpeed : _walkSpeed;

            var velocity = (transform.forward * _currentDirection.y + transform.right * _currentDirection.x) * speed + Vector3.up * _velocityY;

            _characterController.Move(velocity * Time.deltaTime);
        }

        public void SetSprintSpeed(float value)
        {
            _sprintSpeed = value;
        }
    }
}