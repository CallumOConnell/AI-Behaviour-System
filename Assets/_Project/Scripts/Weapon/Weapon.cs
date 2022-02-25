using AI;
using Input;
using Perception;
using System.Collections;
using UI;
using UnityEngine;
using Utility;

namespace Weapons
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private WeaponStats _weaponStats;
        [SerializeField] private WeaponManager _weaponManager;

        [SerializeField] private HUD _hud;

        [SerializeField] private Camera _camera;

        [SerializeField] private Transform _shootPoint;
        [SerializeField] private Transform _casingExitPosition;
        [SerializeField] private Transform _dynamicObjects;

        [SerializeField] private GameObject _hitEffectPrefab;
        [SerializeField] private GameObject _bloodEffectPrefab;
        [SerializeField] private GameObject _impactEffectPrefab;
        [SerializeField] private GameObject _muzzleFlashPrefab;
        [SerializeField] private GameObject _casingPrefab;

        public WeaponStats WeaponStats => _weaponStats;

        public FireMode CurrentFireMode => _currentFireMode;

        private AudioSource _audioSource;

        private Animator _animator;

        private Controls _inputActions;

        private FireMode _currentFireMode;

        private float _timeTillNextFire = 0f;

        private int _loadedMagazineRoundCount = 0;
        private int _spareRounds = 0;

        private bool _isShooting = false;
        private bool _isReloading = false;

        public void AddAmmo(int amount)
        {
            _spareRounds += amount;

            _hud.UpdateSpareAmmo(_spareRounds);
        }

        public bool CanShoot()
        {
            if (Time.time < _timeTillNextFire) return false;
            if (_isShooting) return false;
            if (_isReloading) return false;
            if (_weaponManager.CurrentWeapon == null) return false;

            if (_loadedMagazineRoundCount < 1)
            {
                if (_audioSource != null && !_audioSource.isPlaying && _weaponStats.DryfireSound != null)
                {
                    _audioSource.PlayOneShot(_weaponStats.DryfireSound, 1f);
                }

                return false;
            }

            return true;
        }

        public void TryShoot()
        {
            if (CanShoot())
            {
                if (_currentFireMode == FireMode.Burst)
                {
                    StartCoroutine(BurstShot());
                }

                Shoot();
            }
        }

        public void Shoot()
        {
            _isShooting = true;

            _animator.SetTrigger("Fire");

            if (_muzzleFlashPrefab != null)
            {
                var tempFlash = Instantiate(_muzzleFlashPrefab, _shootPoint.position, _shootPoint.rotation, _dynamicObjects);

                Destroy(tempFlash, 2f);
            }

            ReleaseCasing();

            _timeTillNextFire = Time.time + _weaponStats.FireRate;

            if (_audioSource != null && _weaponStats.ShootSound != null)
            {
                _audioSource.PlayOneShot(_weaponStats.ShootSound, 1f);
            }

            _loadedMagazineRoundCount--;

            if (_hud != null)
            {
                _hud.UpdateAmmo(_loadedMagazineRoundCount, _weaponStats.MagazineSize);
            }

            var successfulHit = Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _weaponStats.BulletRange);

            if (successfulHit)
            {
                _weaponManager.PerceptionManager.AcceptStimulus(new Stimulus(StimulusTypes.AudioWeapon, gameObject, gameObject.transform.position, _shootPoint.transform.forward, 25f, null));

                // Check if hit a zombie
                if (hit.collider.CompareTag(Tags.Enemy))
                {
                    if (_bloodEffectPrefab != null)
                    {
                        var bloodEffect = Instantiate(_bloodEffectPrefab, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal), _dynamicObjects);

                        Destroy(bloodEffect, 1f);
                    }
                    
                    var entity = hit.collider.gameObject;

                    if (entity != null)
                    {
                        var zombie = entity.GetComponent<Zombie>();

                        if (zombie != null)
                        {
                            zombie.Health.TakeDamage(_weaponStats.DamagePerShot);
                        }
                    }
                }

                // Check if hit static object
                if (hit.collider.CompareTag(Tags.Prop))
                {
                    if (_impactEffectPrefab != null)
                    {
                        var bulletImpactDecal = Instantiate(_impactEffectPrefab, hit.point + new Vector3(0f, 0f, -0.02f), Quaternion.FromToRotation(Vector3.forward, hit.normal), _dynamicObjects);

                        Destroy(bulletImpactDecal, 2f);
                    }
                }

                if (_hitEffectPrefab != null)
                {
                    var hitParticalEffect = Instantiate(_hitEffectPrefab, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal), _dynamicObjects);

                    Destroy(hitParticalEffect, 1f);
                }
            }

            _isShooting = false;
        }

        public void TryReload()
        {
            if (!_isReloading && _spareRounds > 0 && _loadedMagazineRoundCount < _weaponStats.MagazineSize)
            {
                StartCoroutine(StartReload());
            }
        }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _animator = GetComponent<Animator>();

            _inputActions = InputManager.InputActions;

            _inputActions.Player.Reload.performed += _ => TryReload();
            _inputActions.Player.Firemode.performed += _ => CycleFireMode();
            _inputActions.Player.Shoot.performed += _ => TryShoot();
        }

        private IEnumerator BurstShot()
        {
            for (var i = 0; i < _weaponStats.BurstLength; i++)
            {
                yield return new WaitForSeconds(_weaponStats.FireRate);

                if (CanShoot())
                {
                    Shoot();
                }
            }
        }

        private IEnumerator StartReload()
        {
            if (_audioSource != null && _weaponStats.ReloadSound != null)
            {
                _isReloading = true;

                _audioSource.PlayOneShot(_weaponStats.ReloadSound, 1f);

                _weaponManager.PerceptionManager.AcceptStimulus(new Stimulus(StimulusTypes.AudioReload, gameObject, gameObject.transform.position, _shootPoint.transform.forward, 25f, null));

                yield return new WaitForSeconds(_weaponStats.ReloadSound.length);

                Reload();
            }
        }

        private void Reload()
        {
            var requiredAmmo = _weaponStats.MagazineSize - _loadedMagazineRoundCount;

            if (_spareRounds >= requiredAmmo)
            {
                _loadedMagazineRoundCount += requiredAmmo;
                _spareRounds -= requiredAmmo;
            }
            else
            {
                _loadedMagazineRoundCount += _spareRounds;
                _spareRounds = 0;
            }

            _hud.UpdateAmmo(_loadedMagazineRoundCount, _weaponStats.MagazineSize);
            _hud.UpdateSpareAmmo(_spareRounds);

            _isReloading = false;
        }

        private void CycleFireMode()
        {
            if (_weaponManager != null && _weaponManager.CurrentWeapon != null)
            {
                for (var i = 0; i < 3; i++)
                {
                    _currentFireMode = ((int)_currentFireMode < 2) ? _currentFireMode + 1 : 0;

                    switch (_currentFireMode)
                    {
                        case FireMode.Single:
                        {
                            if (_weaponStats.SemiCapable)
                            {
                                _hud.UpdateFiremode(_currentFireMode);

                                return;
                            }

                            break;
                        }

                        case FireMode.Burst:
                        {
                            if (_weaponStats.BurstCapable)
                            {
                                _hud.UpdateFiremode(_currentFireMode);

                                return;
                            }

                            break;
                        }

                        case FireMode.Auto:
                        {
                            if (_weaponStats.AutoCapable)
                            {
                                _hud.UpdateFiremode(_currentFireMode);

                                return;
                            }

                            break;
                        }
                    }
                }
            }
        }

        private void ReleaseCasing()
        {
            if (_casingPrefab == null) return;

            var tempCasing = Instantiate(_casingPrefab, _casingExitPosition.position, _casingExitPosition.rotation, _dynamicObjects);

            var rb = tempCasing.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(Random.Range(_weaponStats.EjectionPower * 0.7f, _weaponStats.EjectionPower), _casingExitPosition.position - _casingExitPosition.right * 0.3f - _casingExitPosition.up * 0.6f, 1f);
                rb.AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);
            }

            Destroy(tempCasing, 2f);
        }
    }
}