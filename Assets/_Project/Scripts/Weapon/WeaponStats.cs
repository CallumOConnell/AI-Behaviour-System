using UnityEngine;

namespace Weapons
{
    [CreateAssetMenu(menuName = "WeaponSystem/Weapon", fileName = "Weapon")]
    public class WeaponStats : ScriptableObject
    {
        public float FireRate = 0f;
        public float BulletRange = 0f;
        public float EjectionPower = 0f;

        public int MagazineSize = 0;
        public int DamagePerShot = 0;
        public int BurstLength = 0;

        public bool SemiCapable = true;
        public bool AutoCapable = false;
        public bool BurstCapable = false;

        public AudioClip ShootSound;
        public AudioClip DryfireSound;
        public AudioClip ReloadSound;

        public Vector3 PositionOffset;
        public Vector3 RotationOffset;
    }
}