using System.Collections.Generic;
using UnityEngine;

namespace Perception
{
    public class Envelope
    {
        public GameObject Source;
        public float PerceptionValue;
        public bool MarkEnvelopeForDeletion = false;

        private List<Event> _events;

        public Envelope(Stimulus stimulus, Configuration config)
        {
            Source = stimulus.Source;
            _events = new List<Event>();
            MarkEnvelopeForDeletion = false;
            Add(stimulus, config);
        }

        public void Add(Stimulus stimulus, Configuration config)
        {
            _events.Add(new Event(stimulus, config));
            Process();
        }

        public bool Exists(Stimulus stimulus) => _events.Exists(e => e.Source == stimulus.Source && e.Type == stimulus.Type);

        public void Process()
        {
            PerceptionValue = 0f;

            foreach (var e in _events)
            {
                // Update event's perception value
                e.Process();
                // Update evelope's perception value
                if (!e.MarkEventForDeletion)
                {
                    PerceptionValue += e.EventPerceptionValue;
                }
            }

            // Remove all events marked for deletion
            _events.RemoveAll(e => e.MarkEventForDeletion);

            // If there are no events left mark the envelop for deletion
            if (_events.Count == 0)
            {
                MarkEnvelopeForDeletion = true;
            }
        }

        public void Update(Stimulus stimulus)
        {
            var foundEvent = _events.Find(e => e.Source == stimulus.Source && e.Type == stimulus.Type);

            if (foundEvent != null)
            {
                foundEvent.Update();
                Process();
            }
        }

        public bool CanSeeTarget()
        {
            foreach (var e in _events)
            {
                if (e.Type == StimulusTypes.VisualCanSee)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HeardSound(out Vector3 soundOrigin)
        {
            foreach (var e in _events)
            {
                if (e.Type == StimulusTypes.AudioWeapon || e.Type == StimulusTypes.AudioReload)
                {
                    soundOrigin = e.Location;

                    return true;
                }
            }

            soundOrigin = Vector3.zero;

            return false;
        }
    }
}