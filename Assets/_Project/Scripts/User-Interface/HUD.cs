using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Weapons;

namespace UI
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private TMP_Text _spareAmmoText;
        [SerializeField] private TMP_Text _ammoText;
        [SerializeField] private TMP_Text _firemodeText;
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private TMP_Text _staminaText;

        [SerializeField] private Slider _healthBar;
        [SerializeField] private Slider _staminaBar;

        [SerializeField] private GameObject _weaponInfo;
        [SerializeField] private GameObject _ammoInfo;

        public void Toggle(bool status)
        {
            if (_weaponInfo != null && _ammoInfo != null)
            {
                _weaponInfo.SetActive(status);
                _ammoInfo.SetActive(status);
            }
        }

        public void UpdateAmmo(int currentAmmo, int magazineSize)
        { 
            if (_ammoText != null)
            {
                _ammoText.text = $"{currentAmmo} / {magazineSize}";
            }
        }

        public void UpdateSpareAmmo(int value)
        {
            if (_spareAmmoText != null)
            {
                _spareAmmoText.text = $"Ammo: {value}";
            }
        }

        public void UpdateFiremode(FireMode newFiremode)
        {
            if (_firemodeText != null)
            {
                _firemodeText.text = newFiremode.ToString();
            }
        }

        public void UpdateMaxHealth(float value)
        {
            _healthBar.maxValue = value;
        }

        public void UpdateHealth(float value)
        {
            _healthBar.value = value;
            _healthText.text = $"{value}%";
        }

        public void UpdateStamina(float value)
        {
            _staminaBar.value = value;
            _staminaText.text = $"{value}%";
        }
    }
}