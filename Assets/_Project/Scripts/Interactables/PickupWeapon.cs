using UnityEngine;
using Weapons;

namespace Interaction
{
    public class PickupWeapon : InteractableBase
    {
        [SerializeField] private WeaponManager _weaponManager;

        public override void OnInteract()
        {
            base.OnInteract();

            if (_weaponManager != null)
            {
                if (_weaponManager.CurrentWeapon == null)
                {
                    _weaponManager.PickupGun(gameObject);
                }
                else
                {
                    Debug.Log("Cannot pickup weapon because player already has a weapon");
                }
            }
        }
    }
}