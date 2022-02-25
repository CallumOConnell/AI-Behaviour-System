using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Perception
{
    public class TargetTrackingManager : MonoBehaviour
    {
        [SerializeField] private PerceptionManager _perceptionManager;

        public const int NEUTRAL = 1;
        public const int FRIENDLY = 2;
        public const int HOSTILE = 4;

        public AgentTypes AgentType = AgentTypes.Hostile;

        [HideInInspector] public int AgentFilter;
        [HideInInspector] public TargetResponses Response;
        [HideInInspector] public GameObject Target;

        private const float PROCESS_TIME = 1f;

        private List<Envelope> _envelopes;

        private ZombieBehaviourTree _zombieBehaviour;

        private void Awake()
        {
            Response = TargetResponses.Roam;
            AgentFilter = HOSTILE;
            _envelopes = new List<Envelope>();
            _zombieBehaviour = GetComponent<ZombieBehaviourTree>();
        }

        private void Start()
        {
            _perceptionManager.Register(gameObject);

            StartCoroutine(Process());
        }

        // Return the configuration for a stimulus
        private Configuration GetConfiguration(Stimulus stimulus)
        {
            foreach (var config in _perceptionManager.Configurations)
            {
                if (config.Type == stimulus.Type)
                {
                    return config;
                }
            }

            return null;
        }

        private IEnumerator Process()
        {
            yield return new WaitForSeconds(PROCESS_TIME);

            foreach (var envelope in _envelopes)
            {
                envelope.Process();
            }

            _envelopes.RemoveAll(envelope => envelope.MarkEnvelopeForDeletion);

            StartCoroutine(Process());
        }

        public void AcceptFilteredStimulus(Stimulus stimulus)
        {
            Debug.Log($"Type: {stimulus.Type}, Source: {stimulus.Source.name}", gameObject);

            foreach (var envelope in _envelopes)
            {
                if (envelope.Source == stimulus.Source)
                {
                    if (envelope.Exists(stimulus))
                    {
                        envelope.Update(stimulus);
                    }
                    else
                    {
                        envelope.Add(stimulus, GetConfiguration(stimulus));
                    }
                }
            }

            _envelopes.Add(new Envelope(stimulus, GetConfiguration(stimulus)));
        }

        public TargetResponses GetResponse()
        {
            var highestPerceptionValue = 0f;

            Envelope temp = null;

            if (_envelopes.Count == 0)
            {
                Target = null;
                return TargetResponses.Roam;
            }

            foreach (var envelope in _envelopes)
            {
                if (envelope.PerceptionValue >= highestPerceptionValue)
                {
                    temp = envelope;
                    highestPerceptionValue = envelope.PerceptionValue;
                }
            }

            if (temp.Source.CompareTag(Tags.Player))
            {
                Target = temp.Source;

                if (temp.CanSeeTarget())
                {
                    return TargetResponses.Chase;
                }

                if (temp.HeardSound(out Vector3 soundOrigin))
                {
                    if (_zombieBehaviour != null)
                    {
                        _zombieBehaviour.Tree.Blackboard.SoundOrigin = soundOrigin;
                    }

                    return TargetResponses.Investigate;
                }
            }

            return TargetResponses.Roam;
        }
    }
}