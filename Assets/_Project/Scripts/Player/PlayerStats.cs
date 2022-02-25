using UnityEngine;
using UI;

namespace Player
{
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] private float _maximumHealth = 0f;

        [SerializeField] private HUD _hud;

        public float CurrentHealth { get; private set; } = 0f;

        public bool IsDead { get; private set; }

        private void Awake()
        {
            CurrentHealth = _maximumHealth;

            _hud.UpdateMaxHealth(_maximumHealth);
            _hud.UpdateHealth(CurrentHealth);
        }

        private void Die()
        {
            IsDead = true;
        }

        public void AddHealth(float value)
        {
            CurrentHealth += value;

            if (CurrentHealth > _maximumHealth)
            {
                CurrentHealth = _maximumHealth;
            }

            _hud.UpdateHealth(CurrentHealth);
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage;

            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;

                Die();
            }

            _hud.UpdateHealth(CurrentHealth);
        }
    }
}