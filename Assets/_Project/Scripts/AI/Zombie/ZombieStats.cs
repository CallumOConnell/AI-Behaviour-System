using UnityEngine;

namespace AI
{
    [CreateAssetMenu(menuName = "AI/Zombie", fileName = "Zombie")]
    public class ZombieStats : ScriptableObject
    {
        public float ViewingAngle;
        public float SightRange;
        public float AttackRange;
        public float AttackRate;
        public float MaximumHealth;

        public int AttackDamage;

        public LayerMask PlayerMask;
        public LayerMask EnemyMask;
        public LayerMask CoverMask;
    }
}