using UnityEngine;
using Weapons;

namespace Interaction
{
    public class AmmoBox : InteractableBase
    {
        [SerializeField] private WeaponManager _weaponManager;

        [SerializeField] private int _amount = 1;

        public override void OnInteract()
        {
            base.OnInteract();

            if (_weaponManager != null)
            {
                _weaponManager.CurrentWeapon.AddAmmo(_amount);

                Destroy(gameObject);
            }
        }
    }
}