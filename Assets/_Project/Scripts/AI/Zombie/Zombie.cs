using Perception;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class Zombie : MonoBehaviour
    {
        [SerializeField] private ZombieStats _stats;

        [SerializeField] private PerceptionManager _perceptionManager;

        [SerializeField] private GameObject _target;

        public NavMeshAgent Agent => _agent;
        public PerceptionManager PerceptionManager => _perceptionManager;
        public TargetTrackingManager TargetTrackingManager => _targetTrackingManager;
        public ZombieStats Stats => _stats;
        public ZombieHealth Health => _zombieHealth;

        private NavMeshAgent _agent;
        private TargetTrackingManager _targetTrackingManager;
        private ZombieBehaviourTree _zombieBehaviour;
        private ZombieHealth _zombieHealth;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _targetTrackingManager = GetComponent<TargetTrackingManager>();
            _zombieBehaviour = GetComponent<ZombieBehaviourTree>();
            _zombieHealth = GetComponent<ZombieHealth>();
        }

        private void Start()
        {
            if (_zombieBehaviour != null)
            {
                _zombieBehaviour.Tree.Blackboard.Target = _target;
            }
        }
    }
}