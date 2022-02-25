using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utility;

namespace Perception
{
    public class PerceptionManager : MonoBehaviour
    {
        public List<Configuration> Configurations { get; private set; }

        private List<GameObject> _registrants;
        private List<GameObject> _characters;
        private List<Stimulus> _stimuliBuffer;

        private void Awake()
        {
            _registrants = new List<GameObject>();
            _stimuliBuffer = new List<Stimulus>();
            _characters = GetAllCharacters();

            Configurations = Configuration.GetConfigurations();
        }

        private void Start() => StartCoroutine(Process());

        private List<GameObject> GetAllCharacters()
        {
            var tempFriendly = GameObject.FindGameObjectsWithTag(Tags.Friendly);
            var tempEnemy = GameObject.FindGameObjectsWithTag(Tags.Enemy);
            var tempPlayer = GameObject.FindGameObjectWithTag(Tags.Player);

            var characters = new List<GameObject>(tempFriendly);

            characters.AddRange(tempEnemy);
            characters.Add(tempPlayer);

            return characters;
        }

        private void ProcessStimulationBuffer()
        {
            foreach (var registrant in _registrants)
            {
                var ttm = registrant.GetComponent<TargetTrackingManager>();

                foreach (var stimuli in _stimuliBuffer)
                {
                    if (Filter(ttm, registrant, stimuli))
                    {
                        if (stimuli.Type == StimulusTypes.AudioReload || stimuli.Type == StimulusTypes.AudioWeapon)
                        {
                            var range = Mathf.Abs((stimuli.Location - registrant.transform.position).magnitude);

                            if (range < stimuli.Radius)
                            {
                                ttm.AcceptFilteredStimulus(stimuli);
                            }
                        }
                        else if (stimuli.Type == StimulusTypes.VisualCanSee && stimuli.SecondarySource == registrant)
                        {
                            ttm.AcceptFilteredStimulus(stimuli);
                        }
                    }
                }
            }
            
            _stimuliBuffer.Clear();
        }

        private IEnumerator Process()
        {
            yield return new WaitForSeconds(1f);

            GenerateVisualStimulations();
            ProcessStimulationBuffer();
            StartCoroutine(Process());
        }

        private void GenerateVisualStimulations()
        {
            foreach (var registrant in _registrants)
            {
                foreach (var character in _characters)
                {
                    if (CanSeeCharacter(registrant.transform, character.transform, out Vector3 direction) && registrant != character)
                    {
                        _stimuliBuffer.Add(new Stimulus(StimulusTypes.VisualCanSee, character, character.transform.position, direction, 0f, registrant));
                    }
                }
            }
        }

        public static bool CanSeeCharacter(Transform registrant, Transform character, out Vector3 direction)
        {
            var registrantHeight = registrant.GetComponent<NavMeshAgent>().height;

            var stats = registrant.GetComponent<Zombie>().Stats;

            direction = character.position - registrant.position;

            var distanceToTarget = Vector3.Distance(registrant.position, character.position);

            if (stats.SightRange >= distanceToTarget)
            {
                var angle = Vector3.Angle(direction, registrant.forward);

                angle = System.Math.Abs(angle);

                if (angle < (stats.ViewingAngle / 2))
                {
                    LayerMask combinedMask = stats.CoverMask | stats.PlayerMask | stats.EnemyMask;

                    float targetHeight;

                    if (character.CompareTag(Tags.Player))
                    {
                        targetHeight = character.GetComponent<CharacterController>().height / 1.25f;
                    }
                    else
                    {
                        targetHeight = character.GetComponent<NavMeshAgent>().height / 1.25f;
                    }

                    var registrantEyePosition = new Vector3(registrant.position.x, registrant.position.y + registrantHeight, registrant.position.z);

                    var targetBodyPosition = new Vector3(character.transform.position.x, character.transform.position.y + targetHeight, character.transform.position.z);

                    var rayDirection = (targetBodyPosition - registrantEyePosition).normalized;

                    var successfulHit = Physics.Raycast(registrantEyePosition, rayDirection, out RaycastHit hitData, stats.SightRange, combinedMask);

                    Debug.DrawRay(registrantEyePosition, rayDirection * stats.SightRange, Color.cyan);

                    if (successfulHit)
                    {
                        if (hitData.collider.CompareTag(Tags.Player) || hitData.collider.CompareTag(Tags.Friendly) || hitData.collider.CompareTag(Tags.Enemy))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool Filter(TargetTrackingManager tsm, GameObject registrant, Stimulus stimulus)
        {
            // Don't send an agent's own stimuli to itself
            if (stimulus.Source != registrant)
            {
                var destinationAgentType = tsm.AgentType;
                // Who is the agent interested in
                var validTypes = tsm.AgentFilter;

                AgentTypes sourceAgentType;

                if (stimulus.Source.GetComponent<TargetTrackingManager>() == null)
                {
                    sourceAgentType = AgentTypes.Friendly;
                }
                else
                {
                    sourceAgentType = stimulus.Source.GetComponent<TargetTrackingManager>().AgentType;
                }

                if ((sourceAgentType == AgentTypes.Hostile) && (destinationAgentType == AgentTypes.Friendly) && ((validTypes & TargetTrackingManager.HOSTILE) != 0)) return true;
                if ((sourceAgentType == AgentTypes.Hostile) && (destinationAgentType == AgentTypes.Hostile) && ((validTypes & TargetTrackingManager.FRIENDLY) != 0)) return true;
                if ((sourceAgentType == AgentTypes.Hostile) && (destinationAgentType == AgentTypes.Neutral) && ((validTypes & TargetTrackingManager.NEUTRAL) != 0)) return true;

                if ((sourceAgentType == AgentTypes.Friendly) && (destinationAgentType == AgentTypes.Friendly) && ((validTypes & TargetTrackingManager.FRIENDLY) != 0)) return true;
                if ((sourceAgentType == AgentTypes.Friendly) && (destinationAgentType == AgentTypes.Hostile) && ((validTypes & TargetTrackingManager.HOSTILE) != 0)) return true;
                if ((sourceAgentType == AgentTypes.Friendly) && (destinationAgentType == AgentTypes.Neutral) && ((validTypes & TargetTrackingManager.NEUTRAL) != 0)) return true;

                if ((sourceAgentType == AgentTypes.Neutral) && (destinationAgentType == AgentTypes.Friendly) && ((validTypes & TargetTrackingManager.NEUTRAL) != 0)) return true;
                if ((sourceAgentType == AgentTypes.Neutral) && (destinationAgentType == AgentTypes.Hostile) && ((validTypes & TargetTrackingManager.NEUTRAL) != 0)) return true;
                if ((sourceAgentType == AgentTypes.Neutral) && (destinationAgentType == AgentTypes.Neutral) && ((validTypes & TargetTrackingManager.NEUTRAL) != 0)) return true;

                return false;
            }
            else
            {
                return false;
            }
        }

        public void Register(GameObject registry) => _registrants.Add(registry);

        public void Deregister(GameObject registry) => _registrants.Remove(registry);

        public void AcceptStimulus(Stimulus stimulus) => _stimuliBuffer.Add(stimulus);
    }
}