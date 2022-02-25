using Input;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerStamina : MonoBehaviour
    {
        [SerializeField] private Slider _staminaBar;

        [SerializeField] private TMP_Text _staminaText;

        [SerializeField] private int _maxStamina = 100, _staminaFallRate = 1, _staminaFallMultiplier = 5, _staminaRegenRate = 1, _staminaRegenMultiplier = 5, _staminaRegenWaitTime = 2;

        [SerializeField] private float _walkSpeed = 5f, _sprintSpeed = 9f;

        private float _currentStamina = 0f;
        private float _staminaRegenTimer = 0f;

        private PlayerController _playerController;

        private Controls _inputActions;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();

            _inputActions = InputManager.InputActions;

            _currentStamina = _maxStamina;

            _staminaBar.maxValue = _maxStamina;
            _staminaBar.value = _maxStamina;

            _staminaText.text = $"{_maxStamina}%";
        }
        /*
        private void Update()
        {
            var isMoving = _inputActions.Player.Move.activeControl != null;
            var isSprinting = _inputActions.Player.Sprint.activeControl != null;

            if (isMoving && isSprinting) // Player is moving and sprinting so deduct stamina
            {
                var value = Time.deltaTime / _staminaFallRate * _staminaFallMultiplier;

                _staminaBar.value -= value;

                _currentStamina -= value;

                _staminaRegenTimer = 0f;
            }
            else // Player is not sprinting but could be moving or not so regen stamina
            {
                if (_staminaRegenTimer >= _staminaRegenWaitTime)
                {
                    _staminaBar.value += Time.deltaTime / _staminaRegenRate * _staminaRegenMultiplier;

                    _currentStamina += Time.deltaTime / _staminaRegenRate * _staminaRegenMultiplier;
                }
                else
                {
                    _staminaRegenTimer += Time.deltaTime;
                }
            }

            if (_currentStamina >= _maxStamina) // We are over the max stamina value so set to max value
            {
                _staminaBar.value = _maxStamina;

                _currentStamina = _maxStamina;
            }
            else if (_currentStamina <= 0) // We have no stamina left so set player to walking speed
            {
                _staminaBar.value = 0;

                _currentStamina = 0;

                _playerController.SetSprintSpeed(_walkSpeed);
            }
            else if (_currentStamina >= 0) // If player has any stamina above 0 they can sprint
            {
                _playerController.SetSprintSpeed(_sprintSpeed);
            }

            var percentage = Mathf.Round(_currentStamina / _maxStamina * 100);

            _staminaText.text = $"{percentage}%";
        }
        */
        private void Update()
        {
            var isMoving = _inputActions.Player.Move.activeControl != null;
            var isSprinting = _inputActions.Player.Sprint.activeControl != null;

            if (isMoving && isSprinting) // Player is moving and sprinting so deduct stamina
            {
                DrainStamina();
            }
            else // Player is not sprinting but could be moving or not so regen stamina
            {
                if (_staminaRegenTimer >= _staminaRegenWaitTime)
                {
                    RegenStamina();
                }
                else
                {
                    _staminaRegenTimer += Time.deltaTime;
                }
            }
        }

        private void RegenStamina()
        {
            _currentStamina += Time.deltaTime / _staminaRegenRate * _staminaRegenMultiplier;

            CheckStamina();
            UpdateStamina();
        }

        private void DrainStamina()
        {
            _currentStamina -= Time.deltaTime / _staminaFallRate * _staminaFallMultiplier;

            _staminaRegenTimer = 0f;

            CheckStamina();
            UpdateStamina();
        }

        private void CheckStamina()
        {
            if (_currentStamina > _maxStamina) // We are over the max stamina value so set to max value
            {
                _currentStamina = _maxStamina;
            }
            else if (_currentStamina <= 0) // We have no stamina left so set player to walking speed
            {
                _currentStamina = 0;

                _playerController.SetSprintSpeed(_walkSpeed);
            }
            else if (_currentStamina >= 0) // If player has any stamina above 0 they can sprint
            {
                _playerController.SetSprintSpeed(_sprintSpeed);
            }
        }

        private void UpdateStamina()
        {
            if (_staminaBar != null)
            {
                _staminaBar.value = _currentStamina;
            }

            if (_staminaText != null)
            {
                var percentage = Mathf.Round(_currentStamina / _maxStamina * 100);

                _staminaText.text = $"{percentage}%";
            }
        }
    }
}
