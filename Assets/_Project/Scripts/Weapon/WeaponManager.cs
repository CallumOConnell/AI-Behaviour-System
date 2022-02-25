using Input;
using Perception;
using UnityEngine;
using UI;

namespace Weapons
{
    public class WeaponManager : MonoBehaviour
    {
        [SerializeField] private HUD _hud;

        [SerializeField] private PerceptionManager _perceptionManager;

        [SerializeField] private Transform _weaponHolder;

        [SerializeField] private Camera _camera;

        [SerializeField] private float _throwForce = 500f;

        public Weapon CurrentWeapon { get; private set; }

        public PerceptionManager PerceptionManager => _perceptionManager;

        private Controls _inputActions;

        public void PickupGun(GameObject weapon)
        {
            if (weapon != null)
            {
                weapon.GetComponent<Rigidbody>().isKinematic = true;
                weapon.GetComponent<Collider>().enabled = false;
                
                weapon.transform.parent = _weaponHolder;

                CurrentWeapon = weapon.GetComponent<Weapon>();

                if (CurrentWeapon != null)
                {
                    weapon.transform.localPosition = CurrentWeapon.WeaponStats.PositionOffset;
                    weapon.transform.localRotation = Quaternion.Euler(CurrentWeapon.WeaponStats.RotationOffset);
                }

                if (_hud != null)
                {
                    _hud.Toggle(true);
                    _hud.UpdateAmmo(0, CurrentWeapon.WeaponStats.MagazineSize);
                    _hud.UpdateFiremode(CurrentWeapon.CurrentFireMode);
                }
            }
        }

        public void DropGun()
        {
            if (CurrentWeapon == null) return;

            var weapon = CurrentWeapon.gameObject;

            weapon.transform.SetParent(null);

            var rb = weapon.GetComponent<Rigidbody>();

            rb.isKinematic = false;

            weapon.GetComponent<Collider>().enabled = true;

            rb.AddForce(_camera.transform.forward * _throwForce);
            rb.angularVelocity = _camera.transform.forward * _throwForce;

            CurrentWeapon = null;

            if (_hud != null)
            {
                _hud.Toggle(false);
            }            
        }

        private void Awake()
        {
            _inputActions = InputManager.InputActions;

            _inputActions.Player.DropWeapon.performed += _ => DropGun();
        }
    }
}