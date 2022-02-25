using UnityEngine;
using UnityEngine.UI;

namespace AI
{
    public class ZombieHealth : MonoBehaviour
    {
        [SerializeField] private Image _healthBar;

        public float Health { get; private set; }
        public bool IsDead { get; private set; }

        private Zombie _zombie;

        private void Awake()
        {
            _zombie = GetComponent<Zombie>();

            if (_zombie.Stats != null)
            {
                Health = _zombie.Stats.MaximumHealth;
            }
        }

        private void Die()
        {
            IsDead = true;

            _zombie.PerceptionManager.Deregister(gameObject);

            Debug.Log($"{gameObject.name} died");

            Destroy(gameObject);
        }

        public void TakeDamage(int damage)
        {
            Debug.Log($"{gameObject.name} took {damage} damage");

            Health -= damage;

            if (Health <= 0)
            {
                Die();
            }

            if (_healthBar != null)
            {
                _healthBar.fillAmount = Health / _zombie.Stats.MaximumHealth;
            }
        }
    }
}