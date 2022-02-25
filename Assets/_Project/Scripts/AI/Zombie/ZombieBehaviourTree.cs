using BehaviourTree;
using UnityEngine;

namespace AI
{
    public class ZombieBehaviourTree : MonoBehaviour
    {
        private BT _tree;

        public BT Tree => _tree;

        private void Awake()
        {
            _tree = new BT();

            // Roam

            var roam = new SequenceNode();

            var playIdleAnimation = new IdleNode();
            var roamWaitOne = new WaitNode();
            var findRandomPosition = new FindRandomPositionNode();
            var moveToRandomPosition = new MoveToPoint();
            var roamWaitTwo = new WaitNode();
            roamWaitTwo.Duration = 3f;

            roam.Children.Add(playIdleAnimation);
            roam.Children.Add(roamWaitOne);
            roam.Children.Add(findRandomPosition);
            roam.Children.Add(moveToRandomPosition);
            roam.Children.Add(roamWaitTwo);

            // Investigate

            var heardSound = new HeardSoundNode();
            var moveToSoundOrigin = new MoveToSound();
            var waitAtOrigin = new WaitNode();

            var investigate = new SequenceNode();

            investigate.Children.Add(heardSound);
            investigate.Children.Add(moveToSoundOrigin);
            investigate.Children.Add(waitAtOrigin);

            // Attack

            var inRange = new InRangeNode();
            var canAttack = new CanAttackNode();
            var attackPlayer = new AttackNode();

            var attack = new SequenceNode();

            attack.Children.Add(inRange);
            attack.Children.Add(canAttack);
            attack.Children.Add(attackPlayer);

            // Chase

            var canSeePlayer = new CanSeePlayerNode();
            var facePlayer = new FacePlayerNode();
            var chasePlayer = new ChaseNode();
            var chaseWait = new WaitNode();

            var chase = new SequenceNode();

            chase.Children.Add(canSeePlayer);
            chase.Children.Add(facePlayer);
            chase.Children.Add(chasePlayer);
            chase.Children.Add(chaseWait);

            var beAI = new SelectorNode();

            beAI.Children.Add(investigate);
            beAI.Children.Add(attack);
            beAI.Children.Add(chase);
            beAI.Children.Add(roam);

            var repeatNode = new RepeatNode();

            repeatNode.Child = beAI;

            _tree.RootNode = repeatNode;

            _tree.Bind(GetComponent<Zombie>());
        }

        private void Update() => _tree.Update();
    }
}